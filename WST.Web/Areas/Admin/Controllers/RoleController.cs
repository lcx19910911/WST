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
    public class RoleController : BaseAdminController
    {
        public IRoleService IRoleService;

        public RoleController(IRoleService _IRoleService)
        {
            this.IRoleService = _IRoleService;
        }
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult Add(Role entity)
        {
            ModelState.Remove("ID");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                if (IRoleService.IsExits(x => x.Name == entity.Name))
                {
                    return JResult(Core.Code.ErrorCode.system_name_already_exist, "");
                }
                var result = IRoleService.Add(entity);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult Update(Role entity)
        {
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                var model = IRoleService.Find(entity.ID);
                if (model == null || (model != null && model.IsDelete))
                {
                    return DataErorrJResult();
                }

                if (IRoleService.IsExits(x => x.Name == entity.Name && x.ID != entity.ID))
                {
                    return JResult(Core.Code.ErrorCode.system_name_already_exist, "");
                }
                model.Name = entity.Name;
                model.Remark = entity.Remark;
                model.MenuIDStr = entity.MenuIDStr;
                model.OperateStr = entity.OperateStr;
                var result = IRoleService.Update(model);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult UpdateOperate(string ID, string OperateStr)
        {
            var model = IRoleService.Find(ID);
            if (model == null || (model != null && model.IsDelete))
            {
                return DataErorrJResult();
            }
            model.OperateStr = OperateStr;
            var result = IRoleService.Update(model);
            return JResult(result);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult UpdateMenu(string ID, string MenuIDStr)
        {
            var model = IRoleService.Find(ID);
            if (model == null || (model != null && model.IsDelete))
            {
                return DataErorrJResult();
            }
            model.MenuIDStr = MenuIDStr;
            var result = IRoleService.Update(model);
            return JResult(result);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="key"> 搜索项</param>
        /// <param name="value">搜索项</param>
        /// <returns></returns>
        public ActionResult GetPageList(int pageIndex, int pageSize, string name)
        {
            return JResult(IRoleService.GetPageList(pageIndex, pageSize, name));
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Find(string ID)
        {
            return JResult(IRoleService.Find(ID));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(IRoleService.Delete(ids));
        }



        /// <summary>
        /// 获取地区下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSelectItem()
        {
            return JResult(IRoleService.GetSelectList());
        }
    }
}