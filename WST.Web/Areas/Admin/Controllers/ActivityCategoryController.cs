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
    public class ActivityCategoryController : BaseAdminController
    {
        public IActivityCategoryService IActivityCategoryService;

        public ActivityCategoryController(IActivityCategoryService _IActivityCategoryService)
        {
            this.IActivityCategoryService = _IActivityCategoryService;
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
        public JsonResult Add(ActivityCategory entity)
        {
            ModelState.Remove("ID");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                if (IActivityCategoryService.IsExits(x => x.Name == entity.Name))
                {
                    return JResult(Core.Code.ErrorCode.system_name_already_exist, "");
                }
                entity.CreatedTime = entity.UpdatedTime = DateTime.Now;
                var result = IActivityCategoryService.Add(entity);
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
        public JsonResult Update(ActivityCategory entity)
        {
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                var model = IActivityCategoryService.Find(entity.ID);
                if (model == null || (model != null && model.IsDelete))
                {
                    return DataErorrJResult();
                }
                
                if (IActivityCategoryService.IsExits(x => x.Name == entity.Name&&x.ID!=entity.ID))
                {
                    return JResult(Core.Code.ErrorCode.store_city__namealready_exist, "");
                }

                model.Name = entity.Name;
                model.Sort = entity.Sort;
                var result = IActivityCategoryService.Update(model);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
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
            return JResult(IActivityCategoryService.GetPageList(pageIndex, pageSize, name));
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Find(string ID)
        {
            return JResult(IActivityCategoryService.Find(ID));
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(IActivityCategoryService.Delete(ids));
        }

        /// <summary>
        /// 获取地区下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSelectItem()
        {
            return JResult(IActivityCategoryService.GetSelectList());
        }
    }
}