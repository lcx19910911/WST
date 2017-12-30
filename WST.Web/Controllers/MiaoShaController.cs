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
    public class MiaoShaController : BaseShopController
    {
        public IMiaoShaService IMiaoShaService;
        public IUserActivityService IUserActivityService;

        public MiaoShaController(IMiaoShaService _IMiaoShaService, IUserActivityService _IUserActivityService)
        {
            this.IMiaoShaService = _IMiaoShaService;
            this.IUserActivityService = _IUserActivityService;
        }

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Manage(string id)
        {
            if (id.IsNullOrEmpty())
                return View(new MiaoSha());
            else
                return View(IMiaoShaService.Find(id));
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Manage(MiaoSha entity)
        {
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
                    if (entity.EndTime < entity.StartTime)
                    {
                        return JResult(Core.Code.ErrorCode.end_time_error, "");
                    }
                    if (entity.StartTime < DateTime.Now)
                    {
                        return JResult(Core.Code.ErrorCode.start_time_error, "");
                    }
                    entity.IsNeedPay = false;
                    entity.IsNeedReport = false;
                    entity.UserID = LoginUser.ID;
                    entity.CreatedTime = entity.UpdatedTime = DateTime.Now;
                    var guid = Guid.NewGuid().ToString("N");
                    entity.ID = guid;
                    if (IMiaoShaService.Add(entity) > 0)
                        return JResult(entity.ID.IsNullOrEmpty() ? guid : entity.ID);
                    else
                        return DataErorrJResult();
                }
                else
                {
                    if (entity.EndTime < entity.StartTime)
                    {
                        return JResult(Core.Code.ErrorCode.end_time_error, "");
                    }
                    var model = IMiaoShaService.Find(entity.ID);
                    if (model == null || (model != null && model.IsDelete))
                    {
                        return DataErorrJResult();
                    }
                    if (model.UserID != LoginUser.ID)
                    {
                        return DataErorrJResult();
                    }
                    
                    model.PhoneNumber = entity.PhoneNumber;
                    model.Name = entity.Name;
                    model.Picture = entity.Picture;
                    model.StartTime = entity.StartTime;
                    model.EndTime = entity.EndTime;
                    model.Amount = entity.Amount;
                    model.Connact = entity.Connact;
                    model.Rules = entity.Rules;
                    model.MusicUrl = entity.MusicUrl;
                    model.FunctionName = entity.FunctionName;
                    model.OldPrice = entity.OldPrice;
                    model.LessPrice = entity.LessPrice;
                    model.IntroduceTxtJson = entity.IntroduceTxtJson;
                    model.IntroducePicturesJson = entity.IntroducePicturesJson;
                    model.IntroduceVediosJson = entity.IntroduceVediosJson;
                    model.Direction = entity.Direction;
                    model.GoodsItemsJson = entity.GoodsItemsJson;
                    var result = IMiaoShaService.Update(model);
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
            return JResult(IMiaoShaService.GetPageList(pageIndex, pageSize,LoginUser.ID, name,""));
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Find(string ID)
        {
            var model = IMiaoShaService.Find(ID);
            //if (model != null)
            //{
            //    model.PriceList = IMiaoShaPriceService.GetListByMiaoShaID(ID);
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
            var model = IMiaoShaService.Find(id);
            if (model != null && model.UserID == LoginUser.ID)
            {
                IMiaoShaService.Delete(id);
                return Redirect("/shop/actList");
            }
            else
                return Forbidden();
        }

    }
}