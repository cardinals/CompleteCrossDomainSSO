using System.Web.Mvc;

namespace SSOServer.Filter
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AllowCrossSiteAttribute());//允许跨域
            filters.Add(new PermissionFilterAttribute());//授权控制            
        }
    }
}