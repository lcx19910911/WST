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
using WST.Core.Model;
using WST.Core.Helper;

namespace WST.Web.Controllers
{
    public class UserController : BaseUserController
    {
        public IUserService IUserService;
        public IRechargePlanService IRechargePlanService;
        public IPayOrderService IPayOrderService;
        public IPinTuanService IPinTuanService;
        public IPinTuService IPinTuService;
        public IUserActivityService IUserActivityService;
        public IKanJiaService IKanJiaService;
        public IAdviserService IAdviserService;
        public IMiaoShaService IMiaoShaService;

        public UserController(IUserService _IUserService, IRechargePlanService _IRechargePlanService, IPayOrderService _IPayOrderService, IPinTuService _IPinTuService,
            IPinTuanService _IPinTuanService, IUserActivityService _IUserActivityService, IKanJiaService _IKanJiaService, IAdviserService _IAdviserService,
            IMiaoShaService _IMiaoShaService)
        {
            this.IUserService = _IUserService;
            this.IRechargePlanService = _IRechargePlanService;
            this.IPayOrderService = _IPayOrderService;
            this.IUserActivityService = _IUserActivityService;
            this.IPinTuanService = _IPinTuanService;
            this.IPinTuService = _IPinTuService;
            this.IKanJiaService = _IKanJiaService;
            this.IAdviserService = _IAdviserService;
            this.IMiaoShaService = _IMiaoShaService;
        }


        #region 普通用户
        // GET: User
        public ActionResult Index()
        {
            var userModel = IUserService.Find(LoginUser.ID);
            if (userModel != null)
            {
                if ((userModel.IsMember && !LoginUser.IsMember) || (userModel.EndTime < DateTime.Now && LoginUser.IsMember))
                {
                    this.LoginUser = new Core.Model.LoginUser(userModel);
                }
            }
            else
            {
                return RedirectToAction("index", "login");
            }
            return View(userModel);
        }


        #region 购买时间

        // GET: User
        public ActionResult BuyTime()
        {
            var user = IUserService.Find(LoginUser.ID);
            if (user.Password.IsNullOrEmpty())
            {
                return RedirectToAction("PersonData", "user");
            }
            ViewBag.AppId = Params.WeixinAppId;
            string cacheToken = WxPayApi.GetCacheToken(Params.WeixinAppId, Params.WeixinAppSecret);
            ViewBag.TimeStamp = WxPayApi.GenerateTimeStamp();
            ViewBag.NonceStr = WxPayApi.GenerateNonceStr();
            ViewBag.Signature = WxPayApi.GetSignature(Params.SiteUrl, cacheToken, ViewBag.TimeStamp, ViewBag.NonceStr);
            return View(IRechargePlanService.GetList());
        }

        [HttpPost]
        public ActionResult BuyTime(string planId)
        {
            return JResult(IPayOrderService.BuyTime(planId));
        }

        #endregion

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
            return JResult(IUserActivityService.GetPageList(pageIndex, pageSize, targetId, "", LoginUser.ID, "", null,null));
        }



        public ActionResult PersonData()
        {
            var user = IUserService.Find(LoginUser.ID);
            if (user == null && user.IsDelete)
            {
                return DataErorrJResult();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult PersonData(string Password, string StoreName, string AdviserName, string Mobile, int Code)
        {
            var user = IUserService.Find(LoginUser.ID);
            if (user == null && user.IsDelete)
            {
                return DataErorrJResult();
            }
            if (IUserService.IsExits(x => x.Mobile == Mobile && x.IsMember&&x.ID!=user.ID))
            {
                return JResult(Core.Code.ErrorCode.system_phone_already_exist, "");
            }
            if (Code != CacheHelper.Get<int>("mobile_" + Mobile))
            {
                return JResult(Core.Code.ErrorCode.phone_verificationCode_error, "");
            }
            else
            {
                CacheHelper.Remove("mobile_" + Mobile);
            }
            user.Password = Core.Util.CryptoHelper.MD5_Encrypt(Password);
            //user.StoreName = StoreName;
            user.Mobile = Mobile;
            return JResult(IUserService.Update(user));
        }

        public   ActionResult TryFree()
        {
            var user = IUserService.Find(LoginUser.ID);
            if (user == null && user.IsDelete)
            {
                return Forbidden();
            }
            if (user.IsMember || (user.EndTime.HasValue && !user.StartTime.HasValue))
            {
                return RedirectToAction("index", "user");
            }
            if (user.Password.IsNotNullOrEmpty())
            {

                user.EndTime = DateTime.Now.AddDays(1);
                var result = IUserService.Update(user);
                if (result > 0)
                {
                    //刷新时间
                    LoginHelper.CreateUser(new LoginUser(user), Params.UserCookieName);
                    return RedirectToAction("index", "user");
                }
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult TryFree(string Password, string StoreName, string AdviserName, string Mobile,int Code)
        {
            var user = IUserService.Find(LoginUser.ID);
            if (user == null && user.IsDelete)
            {
                return DataErorrJResult();
            }
            if (user.IsMember || (user.EndTime.HasValue && !user.StartTime.HasValue&&user.Password.IsNotNullOrEmpty()))
            {
                return JResult(Core.Code.ErrorCode.user_had_try, "");
            }
            if (Code != CacheHelper.Get<int>("mobile_" + Mobile))
            {
                return JResult(Core.Code.ErrorCode.phone_verificationCode_error, "");
            }
            if (IUserService.IsExits(x => x.Mobile == Mobile && x.IsMember && x.ID != user.ID))
            {
                return JResult(Core.Code.ErrorCode.system_phone_already_exist, "");
            }
            user.Password = Core.Util.CryptoHelper.MD5_Encrypt(Password);
            //user.StoreName = StoreName;
            user.Mobile = Mobile;
            //if (AdviserName.IsNotNullOrEmpty())
            //{
            //    var advserModel = IAdviserService.Find(x => x.Name == AdviserName);
            //    if (advserModel != null && !advserModel.IsDelete)
            //    {
            //        user.AdviserID = advserModel.ID;
            //    }
            //}
            //user.AdviserName = AdviserName;
            user.EndTime = DateTime.Now.AddDays(1);
            var result = IUserService.Update(user);
            if (result > 0)
            {
                //刷新时间
                LoginHelper.CreateUser(new LoginUser(user), Params.UserCookieName);
            }
            return JResult(result);
        }
        #endregion

        #region 活动支持

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult AddShare(string id, TargetCode code)
        {
            if (code == TargetCode.Kanjia)
            {
                var kanjia = IKanJiaService.Find(id);
                if (kanjia == null || kanjia.IsDelete)
                {
                    return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
                }
                kanjia.ShareCount++;
                return JResult(IKanJiaService.Update(kanjia));
            }
            else if (code == TargetCode.Pintuan)
            {
                var pintuan = IPinTuanService.Find(id);
                if (pintuan == null || pintuan.IsDelete)
                {
                    return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
                }
                pintuan.ShareCount++;
                return JResult(IPinTuanService.Update(pintuan));
            }
            else if (code == TargetCode.Miaosha)
            {
                var miaosha = IMiaoShaService.Find(id);
                if (miaosha == null || miaosha.IsDelete)
                {
                    return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
                }
                miaosha.ShareCount++;
                return JResult(IMiaoShaService.Update(miaosha));
            }
            else if (code == TargetCode.Pintu)
            {
                var pintu = IPinTuService.Find(id);
                if (pintu == null || pintu.IsDelete)
                {
                    return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
                }
                pintu.ShareCount++;
                return JResult(IPinTuService.Update(pintu));
            }
            return DataErorrJResult();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="key"> 搜索项</param>
        /// <param name="value">搜索项</param>
        /// <returns></returns>
        public ActionResult GetUserPageList(int pageIndex, int pageSize, string targetId,bool? isPrize,bool? isUser=null,bool? isOther=null,string targetUserId="")
        {
            return JResult(IUserActivityService.GetPageList(pageIndex, pageSize, targetId, "", "", "",isPrize,null,isUser,isOther,targetUserId));
        }
        #endregion

        #region 砍价
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
            if ((model.StartTime < DateTime.Now && model.EndTime > DateTime.Now))
            {
                if (model.ReportCount == model.PrizeCount)
                {
                    return JResult(Core.Code.ErrorCode.prize_not_had, "");
                }
                if (IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID && string.IsNullOrEmpty(x.TargetUserID)))
                {
                    return JResult(Core.Code.ErrorCode.had_join_in, "");
                }
                model.ReportCount++;
                IKanJiaService.Update(model);

                var newId = Guid.NewGuid().ToString("N");
                var result = IUserActivityService.Add(new UserActivity()
                {
                    ID = newId,
                    Code = TargetCode.Kanjia,
                    Openid = LoginUser.Openid,
                    JoinUserID = LoginUser.ID,
                    JoinUserName = name,
                    Amount = model.OldPrice,
                    IsPrize = true,
                    IsUsedOnLine = false,
                    PrizeInfo = $"用户{LoginUser.Account}创建砍价{model.Name}，原价{model.OldPrice}",
                    ShopUserID = model.UserID,
                    Mobile = mobile,
                    TargetID = id,
                    FiledItemJson = filedJson
                });
                if (result > 0)
                {
                    return JResult(newId);
                }
                else
                    return DataErorrJResult();
            }
            else
            {
                return JResult(Core.Code.ErrorCode.activity_time_out, "");
            }
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
            if ((model.StartTime < DateTime.Now && model.EndTime > DateTime.Now))
            {
                if (userActivityModel.JoinUserID == LoginUser.ID)
                {
                    var startTime = DateTime.Now.AddHours(-(model.LimitHour));
                    if (IUserActivityService.IsExits(x => x.TargetID == userActivityModel.TargetID && x.TargetUserID == LoginUser.ID&&x.JoinUserID==LoginUser.ID && x.CreatedTime > startTime))
                    {
                        return JResult(Core.Code.ErrorCode.time_limit_error, "");
                    }
                }
                else
                {
                    if (IUserActivityService.IsExits(x => x.TargetID == userActivityModel.TargetID && x.JoinUserID == LoginUser.ID && !string.IsNullOrEmpty(x.TargetUserID) && x.TargetUserID == userActivityModel.JoinUserID))
                    {
                        return JResult(Core.Code.ErrorCode.had_kanjia, "");
                    }
                }
                var helpCount = IUserActivityService.GetCount(x => x.TargetID == userActivityModel.TargetID && x.TargetUserID == userActivityModel.JoinUserID);
                if (helpCount >= model.CountLimit)
                {
                    return JResult(Core.Code.ErrorCode.count_limit_error, "");
                }
                var isUsed = false;
                //砍价金额
                var kanPrice = (new Random().Next(1, (model.OncePrice * 100).GetInt()) / 100).GetDecimal();
                if (userActivityModel.Amount - kanPrice <= model.LessPrice)
                {
                    kanPrice = userActivityModel.Amount.Value - model.LessPrice;
                    userActivityModel.Amount = model.LessPrice;
                    model.UsedCount++;
                    IKanJiaService.Update(model);
                    isUsed = true;
                }
                else
                {
                    userActivityModel.Amount -= kanPrice;
                }
                //砍到次数限制
                if ((helpCount + 1) >= model.CountLimit && !isUsed)
                {
                    model.UsedCount++;
                    IKanJiaService.Update(model);
                }

                var toKanjiaModel = new UserActivity()
                {
                    Code = TargetCode.Kanjia,
                    TargetUserID = userActivityModel.JoinUserID,
                    JoinUserID = LoginUser.ID,
                    Openid = LoginUser.Openid,
                    IsPrize = false,
                    IsUsedOnLine = false,
                    PrizeInfo = $"用户{LoginUser.Account}帮助用户{userActivityModel.JoinUserName}砍价{kanPrice}，现价{userActivityModel.Amount}",
                    Amount = kanPrice,
                    ShopUserID = model.UserID,
                    JoinUserName = LoginUser.Account,
                    TargetID = userActivityModel.TargetID
                };
                IUserActivityService.Add(toKanjiaModel);
                return JResult(IUserActivityService.Update(userActivityModel));
            }
            else
            {
                return JResult(Core.Code.ErrorCode.activity_time_out, "");
            }
        }



        /// <summary>
        /// 砍价活动页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userActId"></param>
        /// <returns></returns>
        public ActionResult KanJia(string id, string userActId)
        {
            var model = new KanJia();
            if (userActId.IsNotNullOrEmpty())
            {
                var userAct = IUserActivityService.Find(x => x.ID == userActId);
                if (userAct == null)
                {
                    return Forbidden();
                }
                model = IKanJiaService.Find(userAct.TargetID);
                //帮砍价人列表
                ViewBag.KanJiaList = IUserActivityService.GetList(x => x.TargetID == userAct.TargetID && !string.IsNullOrEmpty(x.TargetUserID) && x.TargetUserID == userAct.JoinUserID);
                ViewBag.Price = userAct.Amount;
                ViewBag.JoinUserID = userAct.JoinUserID;
                id = userAct.TargetID;
            }
            else
            {
                var userActModel = IUserActivityService.Find(x => x.TargetID == id && string.IsNullOrEmpty(x.TargetUserID) && x.JoinUserID == LoginUser.ID && x.IsDelete);
                if (userActModel != null)
                {
                    var userAct = IUserActivityService.Find(x => x.ID == userActId);
                    if (userAct == null || userAct.IsDelete)
                    {
                        return Forbidden();
                    }
                    model = IKanJiaService.Find(userAct.TargetID);
                    //帮砍价人列表
                    ViewBag.KanJiaList = IUserActivityService.GetList(x => x.TargetID == userAct.TargetID && !string.IsNullOrEmpty(x.TargetUserID) && x.TargetUserID == userAct.JoinUserID);
                    ViewBag.Price = userAct.Amount;
                    ViewBag.JoinUserID = userAct.JoinUserID;
                    id = userActModel.TargetID;
                }
                else
                {
                    model = IKanJiaService.Find(id);
                    //帮砍价人列表
                    ViewBag.KanJiaList = new List<UserActivity>();
                    ViewBag.Price = model.OldPrice;
                    ViewBag.JoinUserID = "";

                    if (model == null)
                    {
                        return Forbidden();
                    }
                    model.ClickCount++;
                    IKanJiaService.Update(model);
                }
            }

            ViewBag.IsReport = IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID && string.IsNullOrEmpty(x.TargetUserID));
            ViewBag.AppId = Params.WeixinAppId;
            string cacheToken = WxPayApi.GetCacheToken(Params.WeixinAppId, Params.WeixinAppSecret);
            ViewBag.TimeStamp = WxPayApi.GenerateTimeStamp();
            ViewBag.NonceStr = WxPayApi.GenerateNonceStr();
            ViewBag.Signature = WxPayApi.GetSignature(Request.Url.ToString().Split('#')[0], cacheToken, ViewBag.TimeStamp, ViewBag.NonceStr);
            // LogHelper.WriteDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm")} appid={ViewBag.AppId},TimeStamp={ViewBag.TimeStamp},NonceStr={ViewBag.NonceStr},Signature={ViewBag.Signature}");
            return View(model);
        }

        #endregion

        #region 拼团

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
            if ((model.StartTime < DateTime.Now && model.EndTime > DateTime.Now))
            {
                if (IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID))
                {
                    return JResult(Core.Code.ErrorCode.had_join_in, "");
                }
                var itemList = model.PinTuanItemJson.DeserializeJson<List<PinTuanItem>>();
                if (itemList == null || itemList.Count == 0)
                {
                    return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
                }
                var priceList = model.PinTuanItemJson.DeserializeJson<List<PinTuanItem>>().OrderBy(x => x.Count).ToList();
                var countList = priceList.Select(x => x.Count).ToList();
                var price = 0M;
                var count = priceList.Max(x => x.Count);
                if (model.JoinCount >= count)
                {
                    return JResult(Core.Code.ErrorCode.prize_not_had, "");
                }
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

                model.JoinCount++;
                model.ReportCount++;
                IPinTuanService.Update(model);
                return JResult(IUserActivityService.Add(new UserActivity()
                {
                    Code = TargetCode.Pintuan,
                    JoinUserID = LoginUser.ID,
                    Openid = LoginUser.Openid,
                    JoinUserName = name,
                    IsPrize = true,
                    Amount=price,
                    PrizeInfo = $"团购价{price}",
                    ShopUserID = model.UserID,
                    Mobile = mobile,
                    TargetID = id,
                    FiledItemJson = filedJson
                }));
            }else
            {
                return JResult(Core.Code.ErrorCode.activity_time_out, "");
            }
        }



        /// <summary>
        /// 拼团页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Pintuan(string id)
        {
            var model = IPinTuanService.Find(id);
            if (model == null)
            {
                return Forbidden();
            }
            model.ClickCount++;
            IPinTuanService.Update(model);
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
            ViewBag.IsReport = IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID);
            ViewBag.Price = price;
            ViewBag.AppId = Params.WeixinAppId;
            string cacheToken = WxPayApi.GetCacheToken(Params.WeixinAppId, Params.WeixinAppSecret);
            ViewBag.TimeStamp = WxPayApi.GenerateTimeStamp();
            ViewBag.NonceStr = WxPayApi.GenerateNonceStr();
            ViewBag.Signature = WxPayApi.GetSignature(Request.Url.ToString().Split('#')[0], cacheToken, ViewBag.TimeStamp, ViewBag.NonceStr);
            return View(model);
        }
        #endregion

        #region 秒杀


        /// <summary>
        /// 秒杀页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MiaoSha(string id)
        {
            var model = IMiaoShaService.Find(id);
            if (model == null)
            {
                return Forbidden();
            }
            model.ClickCount++;
            IMiaoShaService.Update(model);
            ViewBag.AppId = Params.WeixinAppId;
            string cacheToken = WxPayApi.GetCacheToken(Params.WeixinAppId, Params.WeixinAppSecret);
            ViewBag.TimeStamp = WxPayApi.GenerateTimeStamp();
            ViewBag.NonceStr = WxPayApi.GenerateNonceStr();
            ViewBag.Signature = WxPayApi.GetSignature(Request.Url.ToString().Split('#')[0], cacheToken, ViewBag.TimeStamp, ViewBag.NonceStr);
            ViewBag.IsReport = IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID);
            return View(model);
        }


        /// <summary>
        /// 秒杀
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ToMiaoSha(string id, string name, string mobile)
        {
            return JResult(IMiaoShaService.ToMiaoSha(id, name, mobile));
        }
        #endregion

        #region 拼图
        /// <summary>
        ///参加人数
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ToPinTu(string id)
        {
            var model = IPinTuService.Find(x => x.ID == id);
            if (model == null || model.IsDelete)
            {
                return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
            }
            var itemList = model.PinTuItemJson.DeserializeJson<List<PinTuItem>>();
            if (itemList == null || itemList.Count == 0)
            {
                return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
            }
            if ((model.StartTime < DateTime.Now && model.EndTime > DateTime.Now))
            {
                var result = 0;
                var newId = Guid.NewGuid().ToString("N");
                //如果用户拼图第一关过了
                if (IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID && string.IsNullOrEmpty(x.TargetUserID)))
                {
                    var userAct = IUserActivityService.Find(x => x.TargetID == id && x.JoinUserID == LoginUser.ID && string.IsNullOrEmpty(x.TargetUserID));
                    var count = IUserActivityService.GetCount(x => x.TargetID == id && x.JoinUserID == LoginUser.ID);
                    if (count > itemList.Count)
                    {
                        return JResult(Core.Code.ErrorCode.pintu_count_limit_error, "");
                    }
                    userAct.Amount = itemList[count - 1].Amount;
                    IUserActivityService.Update(userAct);
                    result = IUserActivityService.Add(new UserActivity()
                    {
                        ID = newId,
                        Code = TargetCode.Pintu,
                        Openid = LoginUser.Openid,
                        JoinUserID = LoginUser.ID,
                        TargetUserID = LoginUser.ID,
                        JoinUserName = LoginUser.Account,
                        Amount = itemList[count - 1].Amount,
                        IsPrize = false,
                        IsUsedOnLine = false,
                        PrizeInfo = $"用户{LoginUser.Account}{model.Name}拼图第{count}关卡，当前价格{itemList[count-1].Amount}",
                        ShopUserID = model.UserID,
                        TargetID = id,
                    });
                }
                if (result > 0)
                {
                    return JResult(newId);
                }
                else
                    return DataErorrJResult();
            }
            else
            {
                return JResult(Core.Code.ErrorCode.activity_time_out, "");
            }
        }

        /// <summary>
        ///参加人数
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult JoinPinTu(string id, string mobile, string name, string filedJson)
        {
            var model = IPinTuService.Find(x => x.ID == id);
            if (model == null || model.IsDelete)
            {
                return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
            }
            var itemList = model.PinTuItemJson.DeserializeJson<List<PinTuItem>>();
            if (itemList == null || itemList.Count == 0)
            {
                return JResult(Core.Code.ErrorCode.sys_param_format_error, "");
            }
            if ((model.StartTime < DateTime.Now && model.EndTime > DateTime.Now))
            {
                var result = 0;
                var newId = Guid.NewGuid().ToString("N");
                //如果用户拼图第一关过了
                if (!IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID && string.IsNullOrEmpty(x.TargetUserID)))
                {
                    if (model.ReportCount == model.PrizeCount)
                    {
                        return JResult(Core.Code.ErrorCode.prize_not_had, "");
                    }

                    model.ReportCount++;
                    IPinTuService.Update(model);

                    result = IUserActivityService.Add(new UserActivity()
                    {
                        ID = newId,
                        Code = TargetCode.Pintu,
                        Openid = LoginUser.Openid,
                        JoinUserID = LoginUser.ID,
                        JoinUserName = name,
                        Amount = model.OldPrice,
                        IsPrize = true,
                        IsUsedOnLine = false,
                        PrizeInfo = $"用户{LoginUser.Account}参加{model.Name}拼图，当前价格{itemList[0].Amount}",
                        ShopUserID = model.UserID,
                        Mobile = mobile,
                        TargetID = id,
                        FiledItemJson = filedJson
                    });

                }
                else
                {
                    return JResult(true);
                }
                if (result > 0)
                {
                    return JResult(newId);
                }
                else
                    return DataErorrJResult();
            }
            else
            {
                return JResult(Core.Code.ErrorCode.activity_time_out, "");
            }
        }

        /// <summary>
        /// 砍价活动页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userActId"></param>
        /// <returns></returns>
        public ActionResult PinTu(string id)
        {
            var model = IPinTuService.Find(id);
            if (model == null)
            {
                return Forbidden();
            }
            model.ClickCount++;
            IPinTuService.Update(model);
            var itemList = model.PinTuItemJson.DeserializeJson<List<PinTuItem>>();
            var count = IUserActivityService.GetCount(x => x.TargetID == id && x.JoinUserID == LoginUser.ID && !string.IsNullOrEmpty(x.TargetUserID) && x.TargetUserID == LoginUser.ID);
            //if (count >= itemList.Count)
            //{
            //    return JResult(Core.Code.ErrorCode.pintu_count_limit_error, "");
            //}
            ViewBag.NowCount = count;
            ViewBag.IsReport = IUserActivityService.IsExits(x => x.TargetID == id && x.JoinUserID == LoginUser.ID);
            ViewBag.ItemList = itemList;
            ViewBag.AppId = Params.WeixinAppId;
            string cacheToken = WxPayApi.GetCacheToken(Params.WeixinAppId, Params.WeixinAppSecret);
            ViewBag.TimeStamp = WxPayApi.GenerateTimeStamp();
            ViewBag.NonceStr = WxPayApi.GenerateNonceStr();
            ViewBag.Signature = WxPayApi.GetSignature(Request.Url.ToString().Split('#')[0], cacheToken, ViewBag.TimeStamp, ViewBag.NonceStr);
            return View(model);
        }

        #endregion

        #region 用户中心

        /// <summary>
        /// 钱包
        /// </summary>
        /// <returns></returns>
        public ActionResult Wallet()
        {
            var user = IUserService.Find(LoginUser.ID);
            if (user == null && user.IsDelete)
            {
                return DataErorrJResult();
            }
            ViewBag.PayOrderList = IPayOrderService.GetList(x => x.UserID == LoginUser.ID && !x.IsDelete && x.State == PayState.Success);
            return View(user);
        }


        /// <summary>
        /// 用户活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ActList()
        {

            //var miaoshasList = IUserActivityService.GetList(x => x.Code == TargetCode.Miaosha);
            ////var miaoshaIdList=miaoshaList.Select(x => x.TargetID).Distinct().ToList();

            //var userIdList = miaoshasList.Select(x => x.JoinUserID).Distinct().ToList();
            //var userDic = IUserService.GetDic(x => userIdList.Contains(x.ID));

            //miaoshasList.ForEach(x =>
            //{
            //    if (x.JoinUserID.IsNotNullOrEmpty()&&userDic.ContainsKey(x.JoinUserID))
            //    {
            //        x.Mobile = userDic[x.JoinUserID].Mobile;
            //        IUserActivityService.Update(x);
            //        }
            //});



            var model = new List<Tuple<string, string, string, string, DateTime?, bool, TargetCode>>();

            var actIdList = IUserActivityService.GetList(x => x.JoinUserID == LoginUser.ID && !x.IsDelete && x.IsPrize);
            var kanjiaIdList = actIdList.Where(x => x.Code == TargetCode.Kanjia && string.IsNullOrEmpty(x.TargetUserID)).Select(x => x.TargetID).Distinct().ToList();
            var kanjiaDic = actIdList.Where(x => x.Code == TargetCode.Kanjia && string.IsNullOrEmpty(x.TargetUserID) && !x.IsDelete).ToDictionary(x => x.TargetID);
            var kanjiaList = IKanJiaService.GetList(x => kanjiaIdList.Contains(x.ID)).Select(x =>
            {
                if (kanjiaDic.ContainsKey(x.ID))
                {
                    var item = kanjiaDic[x.ID];
                    return new Tuple<string, string, string, string, DateTime?, bool, TargetCode>(x.Name, x.Picture, x.ID, item.ID, item.UsedTime, item.IsUsedOnLine, TargetCode.Kanjia);
                }
                else
                    return null;
            }).ToList();
            if (kanjiaList != null && kanjiaIdList.Count > 0)
            {
                model.AddRange(kanjiaList);
            }
            var pintuanIdList = actIdList.Where(x => x.Code == TargetCode.Pintuan).Select(x => x.TargetID).Distinct().ToList();
            if (pintuanIdList != null && pintuanIdList.Count > 0)
            {
                var pintuanDic = actIdList.Where(x => x.Code == TargetCode.Pintuan).ToDictionary(x => x.TargetID);
                var pintuanList = IPinTuanService.GetList(x => pintuanIdList.Contains(x.ID)).Select(x =>
                {
                    if (pintuanDic.ContainsKey(x.ID))
                    {
                        var item = pintuanDic[x.ID];
                        return new Tuple<string, string, string, string, DateTime?, bool, TargetCode>(x.Name, x.Picture, x.ID, item.ID, item.UsedTime, item.IsUsedOnLine, TargetCode.Pintuan);
                    }
                    else
                        return null;
                }).ToList();
                model.AddRange(pintuanList);
            }
            var miaoshaIdList = actIdList.Where(x => x.Code == TargetCode.Miaosha).Select(x => x.TargetID).Distinct().ToList();
            if (miaoshaIdList != null && miaoshaIdList.Count > 0)
            {
                var miaoshaDic = actIdList.Where(x => x.Code == TargetCode.Miaosha).ToDictionary(x => x.TargetID);
                var miaoshaList = IMiaoShaService.GetList(x => miaoshaIdList.Contains(x.ID)).Select(x =>
                {
                    if (miaoshaDic.ContainsKey(x.ID))
                    {
                        var item = miaoshaDic[x.ID];
                        return new Tuple<string, string, string, string, DateTime?, bool, TargetCode>(x.Name, x.Picture, x.ID, item.ID, item.UsedTime, item.IsUsedOnLine, TargetCode.Miaosha);
                    }
                    else
                        return null;
                }).ToList();
                model.AddRange(miaoshaList);
            }


            var pintuIdList = actIdList.Where(x => x.Code == TargetCode.Pintu).Select(x => x.TargetID).Distinct().ToList();
            if (pintuIdList != null && pintuIdList.Count > 0)
            {
                var pintuDic = actIdList.Where(x => x.Code == TargetCode.Pintu).ToDictionary(x => x.TargetID);
                var pintuList = IPinTuService.GetList(x => pintuIdList.Contains(x.ID)).Select(x =>
                {
                    if (pintuDic.ContainsKey(x.ID))
                    {
                        var item = pintuDic[x.ID];
                        return new Tuple<string, string, string, string, DateTime?, bool, TargetCode>(x.Name, x.Picture, x.ID, item.ID, item.UsedTime, item.IsUsedOnLine, TargetCode.Pintu);
                    }
                    else
                        return null;
                }).ToList();
                model.AddRange(pintuList);
            }

            ViewBag.List = model.Where(x => x != null).ToList();
            return View();
        }
        #endregion



    }
}