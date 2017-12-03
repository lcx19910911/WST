using System.Web.Mvc;

namespace WST.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default_route",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", controller="Home", id = UrlParameter.Optional },
                namespaces: new string[] { "WST.Web.Areas.Admin.Controllers" }
            );
        }
    }
}