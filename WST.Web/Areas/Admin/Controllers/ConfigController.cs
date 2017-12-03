using WST.Core.Helper;
using WST.IService;
using WST.Model;
using WST.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WST.Core.Extensions;
namespace WST.Web.Areas.Admin.Controllers
{
    public class ConfigController : BaseAdminController
    {

        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult Set(string key,string value)
        {
            if (key.IsNotNullOrEmpty())
            {
                ConfigHelper.SetValue(key, value);
                return JResult(true);
            }
            else
            {
                return JResult(false);
            }
        }

    }
}