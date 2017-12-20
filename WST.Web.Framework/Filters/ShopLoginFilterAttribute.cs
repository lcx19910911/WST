using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WST.IService;
using WST.Core.Model;
using WST.Core.Code;
using WST.Service;
using WST.Core.Util;
using WST.Core.Extensions;

namespace WST.Web.Framework.Filters
{
    /// <summary>
    /// 过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ShopLoginFilterAttribute : ActionFilterAttribute
    {
        public ShopLoginFilterAttribute()
        { }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseShopController;
            string requestUrl = filterContext.HttpContext.Request.Url.ToString();

            if (controller.LoginUser==null)
            {
                RedirectResult redirectResult = new RedirectResult("/login/Index?redirecturl=" + requestUrl);
                filterContext.Result = redirectResult;
            }
            if (!controller.LoginUser.EndTime.HasValue||controller.LoginUser.EndTime<DateTime.Now)
            {
                RedirectResult redirectResult = new RedirectResult("/user/buytime");
                filterContext.Result = redirectResult;
            }

            
        }
    }
}