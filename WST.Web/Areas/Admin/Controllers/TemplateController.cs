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
    public class TemplateController : BaseAdminController
    {
        public ITemplateService ITemplateService;

        public TemplateController(ITemplateService _ITemplateService)
        {
            this.ITemplateService = _ITemplateService;
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
        public JsonResult Add(Template entity)
        {
            ModelState.Remove("ID");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                if (ITemplateService.IsExits(x => x.Name == entity.Name))
                {
                    return JResult(Core.Code.ErrorCode.store_city__namealready_exist, "");
                }
                entity.CreatedTime = entity.UpdatedTime = DateTime.Now;
                return JResult(ITemplateService.Add(entity));
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
        public JsonResult Update(Template entity)
        {
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                var model = ITemplateService.Find(entity.ID);
                if (model == null || (model != null && model.IsDelete))
                {
                    return DataErorrJResult();
                }

                if (ITemplateService.IsExits(x => x.Name == entity.Name && x.ID != entity.ID))
                {
                    return JResult(Core.Code.ErrorCode.store_city__namealready_exist, "");
                }

                model.Name = entity.Name;
                model.CategoryID = entity.CategoryID;
                model.JsonData = entity.JsonData;
                var result = ITemplateService.Update(model);
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
        public ActionResult GetPageList(int pageIndex, int pageSize, string name, string categoryName)
        {
            return JResult(ITemplateService.GetPageList(pageIndex, pageSize, name, categoryName));
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Find(string ID)
        {
            var model = ITemplateService.Find(ID);
            //if (model != null)
            //{
            //    model.PriceList = ITemplatePriceService.GetListByTemplateID(ID);
            //}         
            return JResult(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(ITemplateService.Delete(ids));
        }

    }
}