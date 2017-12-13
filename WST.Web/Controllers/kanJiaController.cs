using WST.IService;
using WST.Model;
using WST.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WST.Core.Extensions;

namespace WST.Web.Controllers
{
    public class KanJiaController : BaseShopController
    {
        public IKanJiaService IKanJiaService;
        public IUserActivityService IUserActivityService;

        public KanJiaController(IKanJiaService _IKanJiaService, IUserActivityService _IUserActivityService)
        {
            this.IKanJiaService = _IKanJiaService;
            this.IUserActivityService = _IUserActivityService;
        }

        public ViewResult Manage(string id)
        {
            if (id.IsNullOrEmpty())
                return View(new KanJia());
            else
                return View(IKanJiaService.Find(id));
        }
        public ViewResult Add()
        {
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Manage(KanJia entity)
        {
            ModelState.Remove("ID");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            ModelState.Remove("IsNeedPay");
            ModelState.Remove("IsNeedReport");
            ModelState.Remove("UserID");
            if (ModelState.IsValid)
            {
                if (entity.ID.IsNullOrEmpty())
                {
                    entity.IsNeedPay = false;
                    entity.IsNeedReport = false;
                    entity.UserID = LoginUser.ID;
                    entity.CreatedTime = entity.UpdatedTime = DateTime.Now;
                    entity.ID = Guid.NewGuid().ToString("N");
                    if (IKanJiaService.Add(entity) > 0)
                        return JResult(entity.ID);
                    else
                        return DataErorrJResult();
                }
                else
                {
                    var model = IKanJiaService.Find(entity.ID);
                    if (model == null || (model != null && model.IsDelete))
                    {
                        return DataErorrJResult();
                    }
                    if (model.UserID != LoginUser.ID)
                    {
                        return DataErorrJResult();
                    }
                    
                    model.Name = entity.Name;
                    model.Picture = entity.Picture;
                    model.StartTime = entity.StartTime;
                    model.EndTime = entity.EndTime;
                    model.Connact = entity.Connact;
                    model.Rules = entity.Rules;
                    model.MusicUrl = entity.MusicUrl;
                    model.FunctionName = entity.FunctionName;
                    model.OldPrice = entity.OldPrice;
                    model.LessPrice = entity.LessPrice;
                    model.OncePrice = entity.OncePrice;
                    model.CountLimit = entity.CountLimit;
                    model.LimitHour = entity.LimitHour;
                    model.PrizeCount = entity.PrizeCount;
                    model.UsedCount = entity.UsedCount;
                    model.GoodsItemsJson = entity.GoodsItemsJson;

                    model.IntroduceTxtJson = entity.IntroduceTxtJson;
                    model.IntroducePicturesJson = entity.IntroducePicturesJson;
                    model.IntroduceVediosJson = entity.IntroduceVediosJson;
                    model.Direction = entity.Direction;
                    var result = IKanJiaService.Update(model);
                    return JResult(result);
                }
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
            return JResult(IKanJiaService.GetPageList(pageIndex, pageSize,LoginUser.ID, name));
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string ID)
        {
            var model = IKanJiaService.Find(ID);
            //if (model != null)
            //{
            //    model.PriceList = IKanJiaPriceService.GetListByKanJiaID(ID);
            //}         
            return JResult(model);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Find(string ID)
        {
            var model = IKanJiaService.Find(ID);
            //if (model != null)
            //{
            //    model.PriceList = IKanJiaPriceService.GetListByKanJiaID(ID);
            //}         
            return JResult(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            var model = IKanJiaService.Find(id);
            if (model != null && model.UserID == LoginUser.ID)
            {
                IKanJiaService.Delete(id);
                return Redirect("/user/actList");
            }
            else
                return Forbidden();
        }
        
    }
}