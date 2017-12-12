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
        public IRechargePlanService IRechargePlanService;
        public IPayOrderService IPayOrderService;
        public IPinTuanService IPinTuanService;
        public IUserActivityService IUserActivityService;
        public IKanJiaService IKanJiaService;

        public UserController(IUserService _IUserService, IRechargePlanService _IRechargePlanService, IPayOrderService _IPayOrderService,
            IPinTuanService _IPinTuanService, IUserActivityService _IUserActivityService, IKanJiaService _IKanJiaService)
        {
            this.IUserService = _IUserService;
            this.IRechargePlanService = _IRechargePlanService;
            this.IPayOrderService = _IPayOrderService;
            this.IUserActivityService = _IUserActivityService;
            this.IPinTuanService = _IPinTuanService;
            this.IKanJiaService = _IKanJiaService;
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

        [HttpPost]
        public ActionResult JoinPintuan(string id)
        {
            return JResult(IPayOrderService.JoinPintuan(id));
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
        ///参加人数
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult JoinPinTuan(string id, string mobile, string name, string filedJson)
        {
            var model = IPinTuanService.Find(x => x.ID == id);
            if (model == null || model.IsDelete)
            {
                return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
            }
            if (model.StartTime < DateTime.Now || model.EndTime > model.EndTime)
            {
                return JResult(Core.Code.ErrorCode.activity_time_out, "");
            }
            if (IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID))
            {
                return JResult(Core.Code.ErrorCode.had_join_in, "");
            }
            var itemList = model.PinTuanItemJson.DeserializeJson<List<PinTuanItem>>();
            if (itemList == null || itemList.Count == 0)
            {
                return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
            }
            model.JoinCount++;
            var priceDic = itemList.OrderBy(x => x.Count).ToDictionary(x => x.Count);
            var price = 0M;
            for (var index = 1; index <= priceDic.Count; index++)
            {
                if (index < priceDic.Count)
                {
                    if (model.JoinCount > priceDic[index - 1].Count && model.JoinCount < priceDic[index].Count)
                    {
                        price = priceDic[index - 1].Amount;
                    }
                }
                else
                {
                    price = priceDic[index - 1].Amount;
                }
            }
            return JResult(IUserActivityService.Add(new UserActivity()
            {
                Code = TargetCode.Pintuan,
                JoinUserID = LoginUser.ID,
                JoinUserName = name,
                IsPrize = false,
                PrizeInfo = $"团购价{price}",
                ShopUserID = model.UserID,
                Mobile = mobile,
                TargetID = id,
                FiledItemJson=filedJson
            }));
        }

        /// <summary>
        ///参加人数
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult JoinKanJia(string id, string mobile, string name, string filedJson)
        {
            var model = IKanJiaService.Find(x => x.ID == id);
            if (model == null || model.IsDelete)
            {
                return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
            }
            if (model.StartTime < DateTime.Now || model.EndTime > model.EndTime)
            {
                return JResult(Core.Code.ErrorCode.activity_time_out, "");
            }
            if (model.UsedCount == model.PrizeCount)
            {
                return JResult(Core.Code.ErrorCode.activity_time_out, "");
            }
            if (IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID))
            {
                return JResult(Core.Code.ErrorCode.had_join_in, "");
            }
            return JResult(IUserActivityService.Add(new UserActivity()
            {
                Code = TargetCode.Kanjia,
                JoinUserID = LoginUser.ID,
                JoinUserName = name,
                Amount=model.OldPrice,
                IsPrize = true,
                IsUsedOnLine = false,
                PrizeInfo = $"用户{LoginUser.Account}创建砍价{model.Name}，原价{model.OldPrice}",
                ShopUserID = model.UserID,
                Mobile = mobile,
                TargetID = id,
                FiledItemJson=filedJson
            }));
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="key"> 搜索项</param>
        /// <param name="value">搜索项</param>
        /// <returns></returns>
        public ActionResult GetActPageList(int pageIndex, int pageSize, string targetId)
        {
            return JResult(IUserActivityService.GetPageList(pageIndex, pageSize, targetId,"", LoginUser.ID, "", null));
        }

        /// <summary>
        ///参加人数
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ToKanJia(string id)
        {
            var userActivityModel = IUserActivityService.Find(x => x.ID == id);
            if (userActivityModel == null || userActivityModel.IsDelete || userActivityModel.Code != TargetCode.Kanjia)
            {
                return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
            }
            var model = IKanJiaService.Find(x => x.ID == userActivityModel.TargetID);
            if (model.StartTime < DateTime.Now || model.EndTime > model.EndTime)
            {
                return JResult(Core.Code.ErrorCode.activity_time_out, "");
            }
            var startTime = DateTime.Now.AddHours(-(model.LimitHour));
            if (IUserActivityService.IsExits(x => x.TargetID == userActivityModel.TargetID && x.TargetUserID ==LoginUser.ID&&x.CreatedTime> startTime))
            {
                return JResult(Core.Code.ErrorCode.time_limit_error, "");
            }
            if (IUserActivityService.GetCount(x => x.TargetID == userActivityModel.TargetID&&!string.IsNullOrEmpty(x.TargetUserID))<model.CountLimit)
            {
                return JResult(Core.Code.ErrorCode.count_limit_error, "");
            }
            userActivityModel.Amount = model.OldPrice - model.OncePrice;
            if (userActivityModel.Amount == model.LessPrice)
            {
                model.UsedCount++;
                IKanJiaService.Update(model);
            }
            var toKanjiaModel = new UserActivity()
            {
                Code = TargetCode.Kanjia,
                TargetUserID=LoginUser.ID,
                IsPrize =false,
                IsUsedOnLine = false,
                PrizeInfo = $"用户{LoginUser.Account}帮助用户{userActivityModel.JoinUserName}创建砍价{model.OncePrice}，现价{userActivityModel.Amount}",
                ShopUserID = model.UserID,
                TargetID = userActivityModel.TargetID
            };
            IUserActivityService.Add(toKanjiaModel);
            return JResult(IUserActivityService.Update(userActivityModel));
        }



        public ActionResult ActList()
        {
            var model = new List<Tuple<string, string, string, DateTime, DateTime, bool, string, TargetCode>>();
            if (LoginUser.IsMember)
            {
                var kanjiaList = IKanJiaService.GetList(x => x.UserID == LoginUser.ID).Select(x =>
                {
                    return new Tuple<string, string, string, DateTime, DateTime, bool, string, TargetCode>(x.Name, x.Picture, x.ID, x.StartTime, x.EndTime, x.IsDelete, "/user/kanjia?id=" + x.ID, TargetCode.Kanjia);
                }).ToList();
                model.AddRange(kanjiaList);

                var pintuanList = IPinTuanService.GetList(x => x.UserID == LoginUser.ID).Select(x =>
                {
                    return new Tuple<string, string, string, DateTime, DateTime, bool, string, TargetCode>(x.Name, x.Picture, x.ID, x.StartTime, x.EndTime, x.IsDelete, "/user/pintuan?id=" + x.ID, TargetCode.Pintuan);
                }).ToList();
                model.AddRange(pintuanList);
            }
            else
            {
                var actIdList = IUserActivityService.GetList(x => x.JoinUserID == LoginUser.ID);
                var kanjiaIdList = actIdList.Where(x => x.Code == TargetCode.Kanjia).Select(x => x.TargetID).ToList();
                var pintuanIdList = actIdList.Where(x => x.Code == TargetCode.Kanjia).Select(x => x.TargetID).ToList();
                var kanjiaList = IKanJiaService.GetList(x => kanjiaIdList.Contains(x.ID)).Select(x =>
                {
                    return new Tuple<string, string, string, DateTime, DateTime, bool, string, TargetCode>(x.Name, x.Picture, x.ID, x.StartTime, x.EndTime, x.IsDelete, "/user/kanjia?id=" + x.ID, TargetCode.Kanjia);
                }).ToList();
                model.AddRange(kanjiaList);

                var pintuanList = IPinTuanService.GetList(x => kanjiaIdList.Contains(x.ID)).Select(x =>
                {
                    return new Tuple<string, string, string, DateTime, DateTime, bool, string, TargetCode>(x.Name, x.Picture, x.ID, x.StartTime, x.EndTime, x.IsDelete, "/user/pintuan?id=" + x.ID, TargetCode.Pintuan);
                }).ToList();
                model.AddRange(pintuanList);
            }
            return View(model);
        }


        // GET: User
        public ActionResult KanJia()
        {
            return View();
        }

        // GET: User
        public ActionResult Pintuan()
        {
            return View();
        }
    }
}