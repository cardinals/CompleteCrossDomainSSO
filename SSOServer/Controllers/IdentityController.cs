using Common;
using SSOServer.Filter;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SSOServer.Controllers
{
    [AllowAnonymous]
    [AllowCrossSiteAttribute]
    public class IdentityController : Controller
    {
        string secret = ConfigHelper.Secret;
        string clientId = ConfigHelper.ClientId;      
       
        public ActionResult Index()
        {
            return View();
        }      

        public ActionResult Verify()
        {
            //判断发送该请求的客户端是否符合条件
            if (Request["Secret"] == secret || Request["ClientId"] == clientId)
            {
                FormsIdentity formsIdentity = HttpContext.User.Identity as FormsIdentity;
                //判断用户是否登录
                if (formsIdentity!=null)
                {
                    string ReturnURL = Request["ReturnURL"] + "?Ticket=SSOServer";
                    return Redirect(ReturnURL);
                }
                else
                {
                    return RedirectToAction("Login", new { ReturnURL = Request["ReturnURL"] });
                }
            }
            else
            {
                return RedirectToAction("Error", "Identity");
            }
        }

        [HttpGet]       
        public ActionResult Login()
        {
            ViewBag.ReturnURL = Request["ReturnURL"];
            return View();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]      
        public ActionResult Login(string userName, string password)
        {
            if ((userName == "qxh" && password == "123") || (userName == "jlp" && password == "123"))
            {
                //如果认证服务器返回票据，则记录
                User user = new User
                {
                    Name = "SSOServer"
                };
                FormsAuthenticationTicket authenticationTicket = FormsAuthenticationHelper.CreateAuthenticationTicket(user);
                FormsAuthenticationHelper.SetAuthCookie(base.HttpContext, authenticationTicket);
                string ReturnURL = Request["ReturnURL"] + "?Ticket=SSOServer";
                return Redirect(ReturnURL);
            }
            return View();
        }

        /// <summary>
        /// 轮询删除主站和分站的cookie，我就不实现了。本项目不是标准的SSO。有机会写一个标准的SSO
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            return null;
        }
        /// <summary>
        /// 错误页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}