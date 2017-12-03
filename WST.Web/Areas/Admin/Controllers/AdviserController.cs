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
    /// 会籍顾问
    /// </summary>
    public class AdviserController : BaseAdminController
    {
        public IAdviserService IAdviserService;

        public AdviserController(IAdviserService _IAdviserService)
        {
            this.IAdviserService = _IAdviserService;
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
        public ActionResult GetPageList(int pageIndex, int pageSize, string name, string mobile)
        {
            return JResult(IAdviserService.GetPageList(pageIndex, pageSize, name, mobile));
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(WST.Model.Adviser entity)
        {
            ModelState.Remove("ID");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            if (ModelState.IsValid)
            {
                if (IAdviserService.IsExits(x => x.Name == entity.Name))
                {
                    return JResult(Core.Code.ErrorCode.user_account_already_exist);
                }
                entity.CreatedTime = entity.UpdatedTime = DateTime.Now;
                var result = IAdviserService.Add(entity);
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
        public ActionResult Update(WST.Model.Adviser entity)
        {
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            if (ModelState.IsValid)
            {
                var model = IAdviserService.Find(entity.ID);
                if (model == null || (model != null && model.IsDelete))
                {
                    return DataErorrJResult();
                }

                if (IAdviserService.IsExits(x => x.Name == entity.Name && x.ID != entity.ID))
                {
                    return JResult(Core.Code.ErrorCode.store_city__namealready_exist, "");
                }

                model.Name = entity.Name;
                model.Mobile = entity.Mobile;
                model.Sex = entity.Sex;
                var result = IAdviserService.Update(model);
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
            return JResult(IAdviserService.Delete(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string id)
        {
            return JResult(IAdviserService.Find(id));
        }
        /// <summary>
        /// 获取地区下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSelectItem()
        {
            return JResult(IAdviserService.GetSelectList());
        }
    }
}