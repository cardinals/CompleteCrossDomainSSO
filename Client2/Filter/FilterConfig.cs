using System.Web.Mvc;

namespace Client2.Filter
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new PermissionFilterAttribute());//授权控制
        }
    }
}