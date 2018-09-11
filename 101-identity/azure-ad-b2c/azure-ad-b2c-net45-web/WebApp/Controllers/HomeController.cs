using Microsoft.Identity.Client;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using VeracityWebApp.Models;
using System.Web;

namespace VeracityWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Claims()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Error(string message)
        {
            ViewBag.Message = message;

            return View("Error");
        }

        [Authorize]
        public async Task<ActionResult> CallAPI()
        {
            string responseString = "";
            try
            {
                // Retrieve the token with the specified scopes
                var scope = new string[] { Startup.ReadTasksScope };
                string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

                TokenCache userTokenCache = new MSALSessionCache(signedInUserID, this.HttpContext).GetMsalCacheInstance();
                ConfidentialClientApplication cca = new ConfidentialClientApplication(Startup.ClientId, Startup.Authority, Startup.RedirectUri, new ClientCredential(Startup.ClientSecret), userTokenCache, null);

                var user = cca.Users.FirstOrDefault();
                if (user == null)
                {
                    HttpContext.GetOwinContext().Authentication.Challenge();
                    return null;
                    //throw new Exception("The User is NULL.  Please clear your cookies and try again.  Specifically delete cookies for 'login.microsoftonline.com'.  See this GitHub issue for more details: https://github.com/Azure-Samples/active-directory-b2c-dotnet-webapp-and-webapi/issues/9");
                }

                AuthenticationResult result = await cca.AcquireTokenSilentAsync(scope, user, Startup.Authority, false);

                var myApi = ConfigurationManager.AppSettings["api:myApiV3Url"];
                var myApiProfileUrl = $"{myApi}my/profile";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, myApiProfileUrl);

                // Add token to the Authorization header and make the request
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                // Handle the response
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        responseString = await response.Content.ReadAsStringAsync();
                        break;
                    case HttpStatusCode.Unauthorized:
                        responseString = $"Please sign in again. {response.ReasonPhrase}";
                        break;
                    default:
                        responseString = $"Error calling API. StatusCode=${response.StatusCode}";
                        break;
                }
            }
            catch (Exception ex)
            {
                responseString = $"Error calling API: {ex.Message}";
                //return ErrorAction("Error reading to do list: " + ex.Message);
            }

            ViewData["Payload"] = $"{responseString}";
            return View();
        }

    }
}