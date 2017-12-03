using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using WST.Web.Framework;
using WST.IService;
using WST.Core.Extensions;
using WST.Core.Web;
using WST.Core;
using WST.Web.Framework.Filters;
using WxPayAPI;
using WST.Core.Util;
using WST.Model;

namespace WST.Web.Controllers
{
    public class UserController : BaseUserController
    {
        public IUserService IUserService;

        public UserController(IUserService _IUserService)
        {
            this.IUserService = _IUserService;
        }

        // GET: User
        public ActionResult Index()
        {
            var user = IUserService.Find(LoginUser.ID);
            if (user != null)
            {
               
            }
            else
            {
                return RedirectToAction("index", "login");
            }
            return View(user);
        }
        // GET: User
        public ActionResult BuyCard()
        {
            var user = IUserService.Find(LoginUser.ID);
            return View(user);
        }

        
        public ActionResult Sign(string storeId)
        {
            return View();
        }
        public ActionResult PerfectInfo()
        {
            ViewBag.isShowFooter = false;
            return View(IUserService.Find(LoginUser.ID));
        }
    }
}