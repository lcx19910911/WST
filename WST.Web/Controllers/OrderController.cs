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
using WST.Model;
using WST.Core.Util;
using WxPayAPI;

namespace WST.Web.Controllers
{
    public class OrderController : BaseController
    {
        public IUserService IUserService;
        public IPayOrderService IPayOrderService;

        public OrderController(IUserService _IUserService, IPayOrderService _IPayOrderService)
        {
            this.IUserService = _IUserService;
            this.IPayOrderService = _IPayOrderService;
        }

        public ActionResult Scan(string orderId)
        {
            var model = IPayOrderService.Find(orderId);
            if (model != null)
            {
                if (model.State == PayState.WaitPay)
                {
                    var obj = OrderQuery.Run("", orderId);
                    if (obj.GetValue("trade_state").ToString() == "SUCCESS")
                    {
                        model.State = PayState.Success;
                        IPayOrderService.SuccessPayOrder(orderId, obj.GetValue("transaction_id").ToString());
                    }
                }
            }
                return DataErorrJResult();
        }

        public ActionResult Detail(string orderId)
        {
            if (LoginHelper.GetCurrentUser() == null)
            {
                return RedirectToAction("Forbidden", "Base");
            }
            ViewBag.isShowFooter = false;
            var model = IPayOrderService.Find(orderId);
            if (model != null)
            {
                if (LoginHelper.GetCurrentUser().ID != model.UserID)
                {
                    return RedirectToAction("Forbidden", "base");
                }
                if (model.State == PayState.WaitPay)
                {
                    var obj = OrderQuery.Run("", orderId);
                    if (obj.GetValue("trade_state").ToString() == "SUCCESS")
                    {
                        model.State = PayState.Success;
                        IPayOrderService.SuccessPayOrder(orderId, obj.GetValue("transaction_id").ToString());
                    }
                    else
                    {
                        IPayOrderService.FailedPayOrder(orderId);
                    }
                }             
                return View(model);
            }
            else
                return RedirectToAction("_505", "base");
        }

    }
}