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

        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult Add(KanJia entity)
        {
            ModelState.Remove("ID");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            ModelState.Remove("IsNeedPay");
            ModelState.Remove("IsNeedReport");
            if (ModelState.IsValid)
            {
                entity.IsNeedPay = false;
                entity.IsNeedReport = false;
                entity.UserID = LoginUser.ID;
                entity.CreatedTime = entity.UpdatedTime = DateTime.Now;
                return JResult(IKanJiaService.Add(entity));
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
        [ValidateInput(false)]
        public JsonResult Update(KanJia entity)
        {
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("IsDelete");
            ModelState.Remove("IsNeedPay");
            ModelState.Remove("IsNeedReport");
            if (ModelState.IsValid)
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

                if (IKanJiaService.IsExits(x => x.Name == entity.Name && x.ID != entity.ID))
                {
                    return JResult(Core.Code.ErrorCode.store_city__namealready_exist, "");
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

                
                model.IntroduceTxtJson = entity.IntroduceVediosJson;
                model.IntroducePicturesJson = entity.IntroducePicturesJson;
                model.IntroduceVediosJson = entity.IntroduceVediosJson;
                model.Direction = entity.Direction;
                var result = IKanJiaService.Update(model);
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
            return JResult(IKanJiaService.GetPageList(pageIndex, pageSize,LoginUser.ID, name));
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
                return JResult(IKanJiaService.Delete(id));
            }
            else
                return DataErorrJResult();
        }
        
    }
}