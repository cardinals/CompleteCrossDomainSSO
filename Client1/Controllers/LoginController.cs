using Common;
using System.Web.Mvc;

namespace Client1.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginOut()
        {
            FormsAuthenticationHelper.Signout();
            var request = HttpContext.Request;
            string authUrl = ConfigHelper.AuthUrl;
            string clientId = ConfigHelper.ClientId;
            string secret = ConfigHelper.Secret;
            string ReturnURL = request.Url.AbsoluteUri;
            string ssoUrl = string.Format("{0}/Identity/LoginOut?ClientId={1}&Secret={2}&ReturnURL={3}",
             authUrl, clientId, secret, ReturnURL);
            return Redirect(ssoUrl);
        }
    }
}