using System;
using System.Web;
using System.Web.Security;

namespace Common
{
    public class FormsAuthenticationHelper
    {
        /// <summary>
        /// 该值可以控制客户端所有的缓存Cookies是否失效。
        /// 注意：该值只能累加
        /// </summary>
        public static readonly int TicketVersion = 1;
        /// <summary>
        /// 创建登陆用户的票据信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static FormsAuthenticationTicket CreateAuthenticationTicket(User user)
        {
            return new FormsAuthenticationTicket(
                TicketVersion,
                user.Name,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false,
                user.Serialize());
        }

        /// <summary>
        /// 设置登录Cookie
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="authenticationTicket"></param>
        public static void SetAuthCookie(HttpContextBase httpContext, FormsAuthenticationTicket authenticationTicket)
        {
            //设置登录用户票据信息（密文）
            string encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
            HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            httpCookie.Path = FormsAuthentication.FormsCookiePath;
            httpCookie.Domain = FormsAuthentication.CookieDomain;
            httpCookie.Expires = DateTime.Now.Add(FormsAuthentication.Timeout);
            httpContext.Response.Cookies.Add(httpCookie);
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        public static void Signout()
        {
            FormsAuthentication.SignOut();
        }
    }
}