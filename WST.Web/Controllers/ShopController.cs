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
        public IPinTuService IPinTuService;
        public IKanJiaService IKanJiaService;
        public IUserActivityService IUserActivityService;
        public IMiaoShaService IMiaoShaService;

        public ShopController(IUserService _IUserService, IRechargePlanService _IRechargePlanService, IPayOrderService _IPayOrderService, IPinTuService _IPinTuService,
            IPinTuanService _IPinTuanService, IUserActivityService _IUserActivityService, IKanJiaService _IKanJiaService, IMiaoShaService _IMiaoShaService)
        {
            this.IUserService = _IUserService;
            this.IRechargePlanService = _IRechargePlanService;
            this.IPayOrderService = _IPayOrderService;
            this.IUserActivityService = _IUserActivityService;
            this.IPinTuanService = _IPinTuanService;
            this.IKanJiaService = _IKanJiaService;
            this.IMiaoShaService = _IMiaoShaService;
            this.IPinTuService = _IPinTuService;
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




        /// <summary>
        /// 参与用户
        /// </summary>
        /// <returns></returns>
        public ActionResult JoinUser()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="key"> 搜索项</param>
        /// <param name="value">搜索项</param>
        /// <returns></returns>
        public ActionResult GetPageList(int pageIndex, int pageSize, string targetId)
        {
            return JResult(IUserActivityService.GetPageList(pageIndex, pageSize, targetId,LoginUser.ID, "","", null,null,true));
        }


        /// <summary>
        ///核销
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult ToUsed(string id)
        {
            var userActivityModel = IUserActivityService.Find(x => x.ID == id);
            if (userActivityModel == null || userActivityModel.IsDelete||userActivityModel.ShopUserID!=LoginUser.ID||!userActivityModel.IsPrize)
            {
                return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
            }
            userActivityModel.IsUsedOnLine = true;
            if (userActivityModel.Code == TargetCode.Pintuan)
            {
                var model = IPinTuanService.Find(x => x.ID == userActivityModel.TargetID);
                if (model == null || model.IsDelete || userActivityModel.Code != TargetCode.Pintuan)
                {
                    return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
                }
                var priceList = model.PinTuanItemJson.DeserializeJson<List<PinTuanItem>>().OrderBy(x => x.Count).ToList();
                var countList = priceList.Select(x => x.Count).ToList();
                var price = 0M;
                for (var index = 1; index <= countList.Count; index++)
                {
                    if (index < countList.Count)
                    {
                        if (model.JoinCount < countList[index - 1])
                        {
                            price = model.OldPrice;
                            break;
                        }
                        if (model.JoinCount == countList[index - 1])
                        {
                            price = priceList[index - 1].Amount;
                            break;
                        }

                        if (model.JoinCount > countList[index - 1] && model.JoinCount < countList[index])
                        {
                            price = priceList[index - 1].Amount;
                            break;
                        }
                    }
                    else
                    {
                        price = priceList[index - 1].Amount;
                    }
                }
                userActivityModel.Amount = price;
                userActivityModel.IsUsedOnLine = true;
                userActivityModel.UsedTime = DateTime.Now;
            }
            else if (userActivityModel.Code == TargetCode.Kanjia)
            {
                var model = IKanJiaService.Find(x => x.ID == userActivityModel.TargetID);
                if (model == null || model.IsDelete || userActivityModel.Code != TargetCode.Kanjia || userActivityModel.TargetUserID.IsNullOrEmpty())
                {
                    return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
                }
                userActivityModel.IsUsedOnLine = true;
                userActivityModel.UsedTime = DateTime.Now;
            }
            else if (userActivityModel.Code == TargetCode.Miaosha)
            {
                var model = IMiaoShaService.Find(x => x.ID == userActivityModel.TargetID);
                if (model == null || model.IsDelete || userActivityModel.Code != TargetCode.Miaosha)
                {
                    return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
                }
                userActivityModel.IsUsedOnLine = true;
                userActivityModel.UsedTime = DateTime.Now;
            }
            else if (userActivityModel.Code == TargetCode.Miaosha)
            {
                var model = IMiaoShaService.Find(x => x.ID == userActivityModel.TargetID);
                if (model == null || model.IsDelete || userActivityModel.Code != TargetCode.Pintu||userActivityModel.TargetUserID.IsNullOrEmpty())
                {
                    return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
                }
                userActivityModel.IsUsedOnLine = true;
                userActivityModel.UsedTime = DateTime.Now;
            }
            return JResult(IUserActivityService.Update(userActivityModel));
        }


        /// <summary>
        /// 商家活动
        /// </summary>
        /// <returns></returns>
        public ActionResult ActList()
        {
            var model = new List<Tuple<string, string, string, DateTime, DateTime, bool, TargetCode>>();

            var kanjiaList = IKanJiaService.GetList(x => x.UserID == LoginUser.ID).Select(x =>
            {
                return new Tuple<string, string, string, DateTime, DateTime, bool, TargetCode>(x.Name, x.Picture, x.ID, x.StartTime, x.EndTime, x.IsDelete, TargetCode.Kanjia);
            }).ToList();
            if (kanjiaList != null && kanjiaList.Count > 0)
            {
                model.AddRange(kanjiaList);
            }
            var pintuanList = IPinTuanService.GetList(x => x.UserID == LoginUser.ID);
            pintuanList.ForEach(x =>
            {
                model.Add(new Tuple<string, string, string, DateTime, DateTime, bool, TargetCode>(x.Name, x.Picture, x.ID, x.StartTime, x.EndTime, x.IsDelete, TargetCode.Pintuan));
            });

            var miaoshaList = IMiaoShaService.GetList(x => x.UserID == LoginUser.ID);
            miaoshaList.ForEach(x =>
            {
                model.Add(new Tuple<string, string, string, DateTime, DateTime, bool, TargetCode>(x.Name, x.Picture, x.ID, x.StartTime, x.EndTime, x.IsDelete, TargetCode.Miaosha));
            });

            var pintuList = IPinTuService.GetList(x => x.UserID == LoginUser.ID);
            pintuList.ForEach(x =>
            {
                model.Add(new Tuple<string, string, string, DateTime, DateTime, bool, TargetCode>(x.Name, x.Picture, x.ID, x.StartTime, x.EndTime, x.IsDelete, TargetCode.Pintu));
            });

            ViewBag.AppId = Params.WeixinAppId;
            string cacheToken = WxPayApi.GetCacheToken(Params.WeixinAppId, Params.WeixinAppSecret);
            ViewBag.TimeStamp = WxPayApi.GenerateTimeStamp();
            ViewBag.NonceStr = WxPayApi.GenerateNonceStr();
            ViewBag.Signature = WxPayApi.GetSignature(Request.Url.ToString().Split('#')[0], cacheToken, ViewBag.TimeStamp, ViewBag.NonceStr);

            ViewBag.List = model.OrderByDescending(x=>x.Item4).ToList();
            return View();
        }
    }
}