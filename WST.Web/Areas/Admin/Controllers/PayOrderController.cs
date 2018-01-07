using WST.IService;
using WST.Model;
using WST.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WST.Web.Areas.Admin.Controllers
{
    public class PayOrderController : BaseAdminController
    {
        public IPayOrderService IPayOrderService;

        public PayOrderController(IPayOrderService _IPayOrderService)
        {
            this.IPayOrderService = _IPayOrderService;
        }
        public ViewResult Index()
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
        public ActionResult GetPageList(int pageIndex, int pageSize, string no, string userName, OrderCode? code, PayState? state, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            return JResult(IPayOrderService.GetPageList(pageIndex, pageSize, no, userName, code, state, createdTimeStart, createdTimeEnd));
        }
        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //public ActionResult Confirm(string ID,string VoucherThum,string VoucherNO, PayCode Type)
        //{
        //    var model = IPayOrderService.Find(ID);
        //    if (model != null&&model.State==PayState.WaitPay)
        //    {
        //        model.State = PayState.Success;
        //        model.VoucherNO = VoucherNO;
        //        model.VoucherThum = VoucherThum;
        //        model.Type = Type;
        //        var result = IPayOrderService.Update(model);
        //        if (result > 0)
        //        {
        //            JResult(IPayOrderService.SuccessPayOrder(ID, VoucherNO));
        //        }
        //    }
        //    return DataErorrJResult();
        //}


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Find(string ID)
        {
            var model = IPayOrderService.Find(ID);
            //if (model != null)
            //{
            //    model.PriceList = IPayOrderPriceService.GetListByPayOrderID(ID);
            //}         
            return JResult(model);
        }
        
    }
}