using Microsoft.Identity.Client;
using System.Threading;
using System.Web;

namespace aad_b2c_web_net.Models
{
    /// <summary>
    /// Based on Microsoft tutorial:
    /// https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-devquickstarts-web-dotnet-susi
    /// </summary>
    public class MSALSessionCache
    {
        private static readonly ReaderWriterLockSlim SessionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        readonly string _cacheId;
        readonly HttpContextBase _httpContext;
        readonly TokenCache _cache = new TokenCache();

        public MSALSessionCache(string userId, HttpContextBase httpcontext)
        {
            // not object, we want the SUB
            _cacheId = userId + "_TokenCache";
            _httpContext = httpcontext;
            Load();
        }
        public TokenCache GetMsalCacheInstance()
        {
            _cache.SetBeforeAccess(BeforeAccessNotification);
            _cache.SetAfterAccess(AfterAccessNotification);
            Load();
            return _cache;
        }
        public void SaveUserStateValue(string state)
        {
            SessionLock.EnterWriteLock();
            _httpContext.Session[_cacheId + "_state"] = state;
            SessionLock.ExitWriteLock();
        }
        public string ReadUserStateValue()
        {
            SessionLock.EnterReadLock();
            var state = (string)_httpContext.Session[_cacheId + "_state"];
            SessionLock.ExitReadLock();
            return state;
        }
        public void Load()
        {
            SessionLock.EnterReadLock();
            _cache.Deserialize((byte[])_httpContext.Session[_cacheId]);
            SessionLock.ExitReadLock();
        }
        public void Persist()
        {
            SessionLock.EnterWriteLock();

            // Optimistically set HasStateChanged to false. We need to do it early to avoid losing changes made by a concurrent thread.
            _cache.HasStateChanged = false;

            // Reflect changes in the persistent store
            _httpContext.Session[_cacheId] = _cache.Serialize();
            SessionLock.ExitWriteLock();
        }
        // Triggered right before MSAL needs to access the cache.
        // Reload the cache from the persistent store in case it changed since the last access.
        void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            Load();
        }
        // Triggered right after MSAL accessed the cache.
        void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (_cache.HasStateChanged)
            {
                Persist();
            }
        }
    }
}