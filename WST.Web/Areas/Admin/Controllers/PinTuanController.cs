using WST.Core.Helper;
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
    /// <summary>
    /// 拼团
    /// </summary>
    public class PinTuanController : BaseAdminController
    {
        public IPinTuanService IPinTuanService;

        public PinTuanController(IPinTuanService _IPinTuanService)
        {
            this.IPinTuanService = _IPinTuanService;
        }
        // GET: 
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <param name="no">编号 - 搜索项</param>
        /// <returns></returns>
        public ActionResult GetPageList(int pageIndex, int pageSize, string name,string userName)
        {
            return JResult(IPinTuanService.GetPageList(pageIndex, pageSize,"", name, userName));
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(WST.Model.PinTuan entity)
        {
            ModelState.Remove("ID");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            if (ModelState.IsValid)
            {
                entity.CreatedTime = entity.UpdatedTime = DateTime.Now;
                var result = IPinTuanService.Add(entity);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public ActionResult Update(WST.Model.PinTuan entity)
        {
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            if (ModelState.IsValid)
            {
                var model = IPinTuanService.Find(entity.ID);
                if (model == null || (model != null && model.IsDelete))
                {
                    return DataErorrJResult();
                }

                if (IPinTuanService.IsExits(x => x.Name == entity.Name && x.ID != entity.ID))
                {
                    return JResult(Core.Code.ErrorCode.system_name_already_exist, "");
                }

                model.Name = entity.Name;
                model.Picture = entity.Picture;
                model.StartTime = entity.StartTime;
                model.EndTime = entity.EndTime;
                model.Amount = entity.Amount;
                model.IsNeedPay = entity.IsNeedPay;
                model.IsNeedReport = entity.IsNeedReport;
                model.Connact = entity.Connact;
                model.Rules = entity.Rules;
                model.MusicUrl = entity.MusicUrl;
                model.FunctionName = entity.FunctionName;
                var result = IPinTuanService.Update(model);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(IPinTuanService.Delete(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string id)
        {
            return JResult(IPinTuanService.Find(id));
        }
        /// <summary>
        /// 获取地区下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSelectItem()
        {
            return JResult(IPinTuanService.GetSelectList());
        }
    }
}