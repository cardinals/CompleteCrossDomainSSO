using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SSOServer.Filter
{
    /// <summary>
    /// 判断登陆用的访问权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
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
               //根据自己需求写
               //.....
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
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            FormsIdentity formsIdentity = httpContext.User.Identity as FormsIdentity;
            if (formsIdentity == null)
            {
                return false;
            }
            //这里可以做授权验证
            return true;
        }

        protected void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}