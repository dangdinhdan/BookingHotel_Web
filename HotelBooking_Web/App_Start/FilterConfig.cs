using System.Web;
using System.Web.Mvc;

namespace HotelBooking_Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    namespace HotelManagementSystem.Filters
    {
        public class SessionAuthorizeAttribute : AuthorizeAttribute
        {
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                // Kiểm tra session
                if (HttpContext.Current.Session["USER"] == null)
                {
                    // Nếu chưa login -> redirect
                    filterContext.Result = new RedirectResult("~/TaiKhoan/Login");
                    return;
                }

                base.OnAuthorization(filterContext);
            }
        }
    }
}
