﻿using WST.IService;
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
    /// 轮播图
    /// </summary>
    public class CarouselController : BaseAdminController
    {
        public ICarouselService ICarouselService;

        public CarouselController(ICarouselService _ICarouselService)
        {
            this.ICarouselService = _ICarouselService;
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
        public JsonResult Add(Carousel entity)
        {
            ModelState.Remove("ID");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                entity.CreatedTime = entity.UpdatedTime = DateTime.Now;
                var result = ICarouselService.Add(entity);
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
        public JsonResult Update(Carousel entity)
        {
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                var model = ICarouselService.Find(entity.ID);
                if (model == null || (model != null && model.IsDelete))
                {
                    return DataErorrJResult();
                }
                

                model.Title = entity.Title;
                model.Sort = entity.Sort;
                model.Url = entity.Url;
                model.Picture = entity.Picture;
                var result = ICarouselService.Update(model);
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
        public ActionResult GetPageList(int pageIndex, int pageSize, string title)
        {
            return JResult(ICarouselService.GetPageList(pageIndex, pageSize, title));
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Find(string ID)
        {
            return JResult(ICarouselService.Find(ID));
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(ICarouselService.Delete(ids));
        }

        /// <summary>
        /// 获取地区下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSelectItem()
        {
            return JResult(ICarouselService.GetSelectList());
        }
    }
}