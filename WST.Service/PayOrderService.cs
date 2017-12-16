
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WST.Core;
using WST.Core.Code;
using WST.Core.Extensions;
using WST.Core.Helper;
using WST.Core.Model;
using WST.Core.Web;
using WST.DB;
using WST.IService;
using WST.Model;
using WST.Domain;
using WxPayAPI;

namespace WST.Service
{
    /// <summary>
    /// 充值方案
    /// </summary>
    public class PayOrderService : BaseService<PayOrder>, IPayOrderService
    {
        public PayOrderService()
        {
            base.ContextCurrent = HttpContext.Current;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        public PageList<PayOrder> GetPageList(int pageIndex, int pageSize,string no, string userName,OrderCode? code, PayState? state, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.PayOrder.Where(x => !x.IsDelete);

                if (no.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.NO.Contains(no));
                }
                if (userName.IsNotNullOrEmpty())
                {
                    var userIDList = db.User.Where(x => !x.IsDelete && x.NickName.Contains(userName)).Select(x => x.ID).ToList();
                    query = query.Where(x => userIDList.Contains(x.UserID));
                }
                if (code != null)
                {
                    query = query.Where(x => x.Code==code);
                }
                if (state != null)
                {
                    query = query.Where(x => x.State == state);
                }
                if (createdTimeStart != null)
                {
                    query = query.Where(x => x.CreatedTime >= createdTimeStart);
                }
                if (createdTimeEnd != null)
                {
                    createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                    query = query.Where(x => x.CreatedTime < createdTimeEnd);
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var selectUserIDList = list.Select(x => x.UserID).ToList();
                var userDic = db.User.Where(x => !x.IsDelete && selectUserIDList.Contains(x.ID)).ToDictionary(x => x.ID, x => x.NickName);
                list.ForEach(x =>
                {
                    if (x.UserID.IsNullOrEmpty() && userDic.ContainsKey(x.UserID))
                    {
                        x.UserName = userDic[x.UserID];
                    }
                    x.TypeStr = x.Type.GetDescription();
                    x.StateStr = x.State.GetDescription();
                });
                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }
        /// <summary>
        /// 余额充值
        /// </summary>
        /// <param name="storeIds"></param>
        /// <param name="inviteNo"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WebResult<string> Recharge(decimal amount)
        {
            if (amount<=0)
            {
                return Result("", ErrorCode.sys_param_format_error);
            }

            using (var db = new DbRepository())
            {
                var orderId = Guid.NewGuid().ToString("N");
                db.PayOrder.Add(new PayOrder()
                {
                    ID = orderId,
                    NO = DateTime.Now.ToString("yyyyMMddhhmmssfff" + new Random().Next(1000, 2000)),
                    Amount = amount,
                    Remark = $"{Client.LoginUser.Account}在{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}充值金额{amount}",
                    Code = OrderCode.Recharge,
                    UserID = Client.LoginUser.ID,
                    State = PayState.WaitPay,
                    Type = PayCode.WechatPay,
                });
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return Result(orderId);
                }
                else
                {
                    return Result("", ErrorCode.sys_fail);
                }
            }
        }
        public WebResult<string> BuyTime(string planId)
        {
            if (planId.IsNullOrEmpty())
            {
                return Result("", ErrorCode.sys_param_format_error);
            }

            using (var db = new DbRepository())
            {

                var rechargePlan = db.RechargePlan.FirstOrDefault(y => !y.IsDelete && y.ID == planId);
                if (rechargePlan == null || rechargePlan.IsDelete)
                {
                    return Result("", ErrorCode.sys_param_format_error);
                }
                var orderId = Guid.NewGuid().ToString("N");
                db.PayOrder.Add(new PayOrder()
                {
                    ID = orderId,
                    NO = DateTime.Now.ToString("yyyyMMddhhmmssfff" + new Random().Next(1000, 2000)),
                    Amount =rechargePlan.Money,
                    Remark = $"{Client.LoginUser.Account}在{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}购买了的套餐{rechargePlan.Name},天数{rechargePlan.Day}，金额{rechargePlan.Money}",
                    Code = OrderCode.Time,
                    TargetID=planId,
                    UserID = Client.LoginUser.ID,
                    State = PayState.WaitPay,
                    Type = PayCode.WechatPay,
                });
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return Result(orderId);
                }
                else
                {
                    return Result("", ErrorCode.sys_fail);
                }
            }
        }


        public WebResult<string> JoinPintuan(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return Result("", ErrorCode.sys_param_format_error);
            }

            using (var db = new DbRepository())
            {
                var userActivityModel = db.UserActivity.FirstOrDefault(y => !y.IsDelete && y.ID == id);
                if (userActivityModel == null || userActivityModel.IsDelete||userActivityModel.Code!=TargetCode.Pintuan)
                {
                    return Result("", ErrorCode.sys_param_format_error);
                }

                var model = db.PinTuan.Find(userActivityModel.TargetID);
                if (model == null || model.IsDelete)
                {
                    return Result("", ErrorCode.sys_param_format_error);
                }
                if (model.StartTime < DateTime.Now || model.EndTime > model.EndTime)
                {
                    return Result("",Core.Code.ErrorCode.activity_time_out);
                }
                if (db.UserActivity.Any(x => x.TargetID == id && x.JoinUserID == Client.LoginUser.ID))
                {
                    return Result("",Core.Code.ErrorCode.had_join_in);
                }
                var orderId = Guid.NewGuid().ToString("N");
                db.PayOrder.Add(new PayOrder()
                {
                    ID = orderId,
                    NO = DateTime.Now.ToString("yyyyMMddhhmmssfff" + new Random().Next(1000, 2000)),
                    Amount = model.Amount,
                    Remark = $"{Client.LoginUser.Account}在{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}支付了团购{model.Name}的拼团，金额{model.Amount}",
                    Code = OrderCode.Activity,
                    TargetID = id,
                    UserID = Client.LoginUser.ID,
                    State = PayState.WaitPay,
                    Type = PayCode.WechatPay,
                });
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return Result(orderId);
                }
                else
                {
                    return Result("", ErrorCode.sys_fail);
                }
            }
        }


        /// <summary>
        /// 支付成功
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> SuccessPayOrder(string id, string thirdOrderId)
        {
            if (id.IsNullOrEmpty()|| thirdOrderId.IsNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            using (var db = new DbRepository())
            {
                var PayOrderModel = db.PayOrder.Find(id);
                if (PayOrderModel == null || PayOrderModel.IsDelete || PayOrderModel.State != PayState.WaitPay)
                {
                    return Result(false, ErrorCode.sys_param_format_error);
                }
                PayOrderModel.State = PayState.Success;
                PayOrderModel.SuccessTime = DateTime.Now;
                PayOrderModel.ThirdOrderID = thirdOrderId;

                var user = db.User.Find(PayOrderModel.UserID);
                if (user == null || user.IsDelete)
                {
                    return Result(false, ErrorCode.sys_param_format_error);
                }

                //是否充值
                if (PayOrderModel.Code == OrderCode.Recharge)
                {
                    user.Balance += PayOrderModel.Amount;
                }
                else if (PayOrderModel.Code == OrderCode.Activity)
                {
                    var userActivityModel = db.UserActivity.Find(PayOrderModel.TargetID);
                    if (userActivityModel == null || userActivityModel.IsDelete)
                    {
                        return Result(false, ErrorCode.sys_param_format_error);
                    }

                    var model = db.PinTuan.Find(userActivityModel.TargetID);
                    if (model == null || model.IsDelete)
                    {
                        return Result(false, ErrorCode.sys_param_format_error);
                    }
                    if (model.StartTime < DateTime.Now || model.EndTime > model.EndTime)
                    {
                        return Result(false, Core.Code.ErrorCode.activity_time_out);
                    }
                    var itemList = model.PinTuanItemJson.DeserializeJson<List<PinTuanItem>>();
                    if (itemList == null || itemList.Count == 0)
                    {
                        return Result(false,Core.Code.ErrorCode.sys_param_format_error);
                    }
                    model.JoinCount++;
                    userActivityModel.IsPrize = true;
                }
                else if (PayOrderModel.Code == OrderCode.Time)
                {
                    if (user.IsMember == false)
                    {
                        user.IsMember = true;
                    }
                    var rechargePlan = db.RechargePlan.FirstOrDefault(y => !y.IsDelete && y.ID == PayOrderModel.TargetID);
                    if (rechargePlan == null || rechargePlan.IsDelete)
                    {
                        return Result(false, ErrorCode.sys_param_format_error);
                    }
                    if (!user.StartTime.HasValue)
                    {
                        user.StartTime = DateTime.Now;
                        user.EndTime = DateTime.Now.AddDays(rechargePlan.Day);
                    }

                    else
                    {
                        if (user.EndTime.Value > DateTime.Now)
                        {
                            user.EndTime = user.EndTime.Value.AddDays(rechargePlan.Day);
                        }
                        else
                        {
                            user.EndTime = DateTime.Now.AddDays(rechargePlan.Day);
                        }
                    }
                    //刷新时间
                    LoginHelper.CreateUser(new LoginUser(user), Params.UserCookieName);
                }
                PayOrderModel.ThirdOrderID = thirdOrderId;
                PayOrderModel.State = PayState.Success;
                PayOrderModel.SuccessTime = DateTime.Now;
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return Result(true);
                }
                else
                {
                    return Result(false, ErrorCode.sys_fail);
                }
            }
        }

        /// <summary>
        /// 充值失败
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WebResult<bool> FailedPayOrder(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            using (var db = new DbRepository())
            {
                var PayOrderModel = db.PayOrder.Find(id);
                if (PayOrderModel == null || PayOrderModel.IsDelete || PayOrderModel.State != PayState.WaitPay)
                {
                    return Result(false, ErrorCode.sys_param_format_error);
                }
                PayOrderModel.State = PayState.Failed;

                var result = db.SaveChanges();
                if (result > 0)
                {
                    return Result(true);
                }
                else
                {
                    return Result(false, ErrorCode.sys_fail);
                }
            }
        }


        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public WebResult<bool> RefundPayOrder(string id)
        //{
        //    if (id.IsNullOrEmpty())
        //    {
        //        return Result(false, ErrorCode.sys_param_format_error);
        //    }
        //    using (var db = new DbRepository())
        //    {
        //        var PayOrderModel = db.PayOrder.Find(id);
        //        if (PayOrderModel == null || PayOrderModel.IsDelete || (PayOrderModel.State != PayState.Success))
        //        {
        //            return Result(false, ErrorCode.sys_param_format_error);
        //        }

        //        var userModel = db.User.Find(PayOrderModel.UserID);
        //        if (userModel == null || userModel.IsDelete )
        //        {
        //            return Result(false, ErrorCode.sys_param_format_error);
        //        }
        //        PayOrderModel.State = PayState.Return;

        //        if (PayOrderModel.Type == PayCode.Recharge)
        //        {

        //            userModel.Balance -= PayOrderModel.Amount;
        //            userModel.TotalRecharge -= PayOrderModel.Amount;
        //        }
        //        else if (PayOrderModel.Type == PayCode.WechatPay)
        //        {
        //            if (PayOrderModel.Code == OrderCode.Recharge)
        //            {
        //                userModel.Balance -= PayOrderModel.Amount;
        //                userModel.TotalRecharge -= PayOrderModel.Amount;
        //            }
        //            else if(PayOrderModel.Code == OrderCode.Recharge)
        //            {

        //            }
        //            Refund.Run("", id, PayOrderModel.Amount.ToString(), PayOrderModel.Amount.ToString());
        //        }
        //        var result = db.SaveChanges();
        //        if (result > 0)
        //        {
        //            return Result(true);
        //        }
        //        else
        //        {
        //            return Result(false, ErrorCode.sys_fail);
        //        }
        //    }
        //}



        /// <summary>
        /// 拿走
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> TakenPayOrder(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            using (var db = new DbRepository())
            {
                var PayOrderModel = db.PayOrder.Find(id);
                if (PayOrderModel == null || PayOrderModel.IsDelete || PayOrderModel.State != PayState.Success)
                {
                    return Result(false, ErrorCode.sys_param_format_error);
                }
                PayOrderModel.State = PayState.Taken;        
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return Result(true);
                }
                else
                {
                    return Result(false, ErrorCode.sys_fail);
                }
            }
        }
        
    }
}
