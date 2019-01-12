using Common;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Client1.Filter
{
    /// <summary>
    /// 判断登陆用的访问权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,Inherited =true,AllowMultiple =true)]
    public class PermissionFilterAttribute: FilterAttribute, IAuthorizationFilter
    {       

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext==null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                throw new InvalidOperationException("AuthorizeAttribute Cannot Use Within Child Action Cache");
            }
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
            {
                return;
            }            
            if (this.AuthorizeCore(filterContext.HttpContext))
            {                
                //...
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }


        /// <summary>
        /// 具体判断方法
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }            
            string ticket = httpContext.Request["Ticket"];
            if (!string.IsNullOrEmpty(ticket))
            {
                //如果认证服务器返回票据，则记录
                User user = new User
                {
                    Name = "Client1"
                };
                FormsAuthenticationTicket authenticationTicket = FormsAuthenticationHelper.CreateAuthenticationTicket(user);
                FormsAuthenticationHelper.SetAuthCookie(httpContext, authenticationTicket);
                return true;
            }            
            FormsIdentity formsIdentity = httpContext.User.Identity as FormsIdentity;
            //验证cookie 用户是否有效
            if (formsIdentity==null)
            {
                return false;
            }
            //这里可以做授权验证
            //....
            return true;
        }

        protected void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //根据webconfig中的配置跳转到Login,具体逻辑在Login处理
            //filterContext.Result = new HttpUnauthorizedResult();           
            var request = filterContext.HttpContext.Request;          
            string authUrl = ConfigHelper.AuthUrl;
            string clientId = ConfigHelper.ClientId;
            string secret = ConfigHelper.Secret;
            string ReturnURL = request.Url.AbsoluteUri;
            filterContext.Result = new RedirectResult(string.Format("{0}/Identity/Verify?ClientId={1}&Secret={2}&ReturnURL={3}",
             authUrl, clientId, secret, ReturnURL));
            return;
        }
    }
}