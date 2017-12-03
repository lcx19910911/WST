
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WST.Core.Model;
using WST.Model;
using WST.Domain;

namespace WST.IService
{
    public interface IPayOrderService : IBaseService<PayOrder>
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        PageList<PayOrder> GetPageList(int pageIndex, int pageSize,string no, string userName,OrderCode? code, PayState? stete, DateTime? createdTimeStart, DateTime? createdTimeEnd);



        WebResult<bool> SuccessPayOrder(string id,string thirdOrderId);

        WebResult<bool> FailedPayOrder(string id);

        /// <summary>
        /// 购买会员卡
        /// </summary>
        /// <param name="storeIds"></param>
        /// <param name="inviteNo"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        WebResult<string> BuyTime(string planId);

        /// <summary>
        /// 余额充值
        /// </summary>
        /// <param name="storeIds"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        WebResult<string> Recharge(decimal amount);

        /// <summary>
        /// 拿走
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        WebResult<bool> TakenPayOrder(string id);
    }
}
