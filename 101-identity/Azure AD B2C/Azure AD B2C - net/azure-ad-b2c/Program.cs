using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace azure_ad_b2c
{
    public class Program
    {
        /// <summary>
        /// Active Directory Tenent where app is registered
        /// </summary>
        private const string Tenant = "<tenant name goes here - format: <some name>.onmicrosoft.com>";
        /// <summary>
        /// Id of registered app. App needs to be registered in tenant.
        /// </summary>
        private const string ClientId = "< id from registered app goes here>";
        /// <summary>
        /// Policy for authentication
        /// </summary>
        public static string PolicySignUpSignIn = "< signup policy goes here >";
        /// <summary>
        /// List of scopes for tenant
        /// openid and offline_access added by default, no need to repeat
        /// </summary>
        public static string[] ApiScopes =
        {
            "< api scope goes here >"
        };
        /// <summary>
        /// Template url where authentication will take place. 
        /// {tenant} and {policy} to be replaced by proper values
        /// </summary>
        private static string BaseAuthority =
            "https://login.microsoftonline.com/tfp/{tenant}/{policy}/oauth2/v2.0/authorize";
        /// <summary>
        /// Propert url where authentication will take place.
        /// </summary>
        public static string Authority =
            BaseAuthority.Replace("{tenant}", Tenant).Replace("{policy}", PolicySignUpSignIn);
        /// <summary>
        /// Identity object performing authentication and recieving access token
        /// </summary>
        public static PublicClientApplication PublicClientApp { get; } =
            new PublicClientApplication(ClientId, Authority, TokenCacheHelper.GetUserCache());
        /// <summary>
        /// Main method. Executes sign in and shows result.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var authResult = SignIn();
            authResult.Wait();
            DisplayBasicTokenInfo(authResult.Result);
            Console.ReadLine();
        }
        /// <summary>
        /// Tries to connect to Authority with given Scopes and Policies.
        /// Results with separate dialog where user needs to specify credentials.
        /// </summary>
        /// <returns>Authentication result containing access token</returns>
        public static async Task<AuthenticationResult> SignIn()
        {
            try
            {
                return await PublicClientApp.AcquireTokenAsync(ApiScopes,
                    GetUserByPolicy(PublicClientApp.Users, PolicySignUpSignIn), UIBehavior.SelectAccount, string.Empty,
                    null, Authority);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Users:{string.Join(",", PublicClientApp.Users.Select(u => u.Identifier))}{Environment.NewLine}Error Acquiring Token:{Environment.NewLine}{ex}");
                return null;
            }
        }
        /// <summary>
        /// Small summary of authentication result object
        /// </summary>
        /// <param name="authResult"></param>
        private static void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            if (authResult == null) return;
            Console.WriteLine("Autorization token info: ");
            Console.WriteLine($"Name: {authResult.User.Name}");
            Console.WriteLine($"Token Expires: {authResult.ExpiresOn.ToLocalTime()}");
            Console.WriteLine($"Access Token: {authResult.AccessToken}");
        }
        /// <summary>
        /// Finds users according to given policy
        /// </summary>
        /// <param name="users"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        private static IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy.ToLower())) return user;
            }
            return null;
        }
        /// <summary>
        /// Decodes url
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }
    }
}