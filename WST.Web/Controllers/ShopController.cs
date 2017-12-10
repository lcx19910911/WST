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
    public class ShopController : BaseShopController
    {
        public IUserService IUserService;
        public IRechargePlanService IRechargePlanService;
        public IPayOrderService IPayOrderService;
        public IPinTuanService IPinTuanService;
        public IUserActivityService IUserActivityService;

        public ShopController(IUserService _IUserService, IRechargePlanService _IRechargePlanService, IPayOrderService _IPayOrderService,
            IPinTuanService _IPinTuanService, IUserActivityService _IUserActivityService)
        {
            this.IUserService = _IUserService;
            this.IRechargePlanService = _IRechargePlanService;
            this.IPayOrderService = _IPayOrderService;
            this.IUserActivityService = _IUserActivityService;
            this.IPinTuanService = _IPinTuanService;
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
        public ActionResult BuyTime()
        {
            return View(IRechargePlanService.GetList());
        }

        [HttpPost]
        public ActionResult BuyTime(string planId)
        {
            return JResult(IPayOrderService.BuyTime(planId));
        }
        /// <summary>
        /// 余额充值
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recharge(decimal amount)
        {
            return JResult(IPayOrderService.Recharge(amount));
        }
    }
}