using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VeracityWebApp.Controllers
{
    public class AccountController : Controller
    {
        /*
         *  Called when requesting to sign up or sign in
         */
        public void SignUpSignIn()
        {
            // Use the default policy to process the sign up / sign in flow
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge();
                return;
            }

            Response.Redirect("/");
        }

        /*
        *  Called when requesting to sign out
        */
        public ActionResult SignOut()
        {
            // To sign out the user, you should issue an OpenIDConnect sign out request.
            if (Request.IsAuthenticated)
            {
                var redirectUrl = Request.Url.ToString();

                IEnumerable<AuthenticationDescription> authTypes = HttpContext.GetOwinContext().Authentication.GetAuthenticationTypes();
                HttpContext.GetOwinContext().Authentication.SignOut(new AuthenticationProperties { RedirectUri = redirectUrl }, authTypes.Select(t => t.AuthenticationType).ToArray());
                Request.GetOwinContext().Authentication.GetAuthenticationTypes();
            }

            return RedirectToAction(nameof(SignedOut), "Account");
        }

        [HttpGet]
        public ActionResult SignedOut()
        {
            return RedirectPermanent("https://www.veracity.com/auth/logout");
        }
    }
}