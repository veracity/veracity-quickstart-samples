using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace aad_b2c_web_net.Controllers
{
    /// <summary>
    /// Based on Microsoft tutorial:
    /// https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-devquickstarts-web-dotnet-susi
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Claims()
        {
            var displayName =
                ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);
            ViewBag.DisplayName = displayName != null ? displayName.Value : string.Empty;
            return View();
        }
    }
}