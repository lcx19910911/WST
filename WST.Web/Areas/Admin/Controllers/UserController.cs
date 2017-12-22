using WST.Core.Helper;
using WST.IService;
using WST.Model;
using WST.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WST.Core.Extensions;

namespace WST.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        public IUserService IUserService;

        public UserController(IUserService _IUserService)
        {
            this.IUserService = _IUserService;
        }
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Member()
        {
            return View();
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult UpdateMember(User entity)
        {
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("EndTime");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                var model = IUserService.Find(entity.ID);
                if (model == null || (model != null && model.IsDelete))
                {
                    return DataErorrJResult();
                }

                //if (IUserService.IsExits(x => x.IDCard == entity.IDCard && x.ID != entity.ID))
                //{
                //    return JResult(Core.Code.ErrorCode.idcard_had_extis, "");
                //}

                if (IUserService.IsExits(x => x.Mobile == entity.Mobile && x.ID != entity.ID))
                {
                    return JResult(Core.Code.ErrorCode.system_phone_already_exist, "");
                }
                //if (entity.IDCard.Length != 18)
                //{

                //    return JResult(Core.Code.ErrorCode.idcard_erroe, "");
                //}
                //model.IDCard = entity.IDCard;
                model.HeadImgUrl = entity.HeadImgUrl;
                model.Mobile = entity.Mobile;
                model.NickName = entity.NickName;
                model.AdviserID = entity.AdviserID;
                model.StoreName = entity.StoreName;
                model.StartTime = entity.StartTime;
                model.EndTime = entity.EndTime;

                var result = IUserService.Update(model);
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
        public JsonResult AddMember(User entity)
        {
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("EndTime");
            ModelState.Remove("AdviserID");
            ModelState.Remove("IsDelete");
            if (ModelState.IsValid)
            {
                var model = IUserService.Find(entity.ID);
                if (model == null || (model != null && model.IsDelete))
                {
                    return DataErorrJResult();
                }

                //if (IUserService.IsExits(x => x.IDCard == entity.IDCard && x.ID != entity.ID))
                //{
                //    return JResult(Core.Code.ErrorCode.idcard_had_extis, "");
                //}

                if (IUserService.IsExits(x => x.Mobile == entity.Mobile && x.ID != entity.ID))
                {
                    return JResult(Core.Code.ErrorCode.system_phone_already_exist, "");
                }
                //if (entity.IDCard.Length != 18)
                //{

                //    return JResult(Core.Code.ErrorCode.idcard_erroe, "");
                //}
                //model.IDCard = entity.IDCard;
                model.HeadImgUrl = entity.HeadImgUrl;
                model.Mobile = entity.Mobile;
                model.NickName = entity.NickName;
                model.AdviserID = entity.AdviserID;
                model.StoreName = entity.StoreName;
                model.StartTime = entity.StartTime;
                model.EndTime = entity.EndTime;
                model.IsMember = true;

                var result = IUserService.Update(model);
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
        public ActionResult GetPageList(int pageIndex, int pageSize, string name, string phone, bool isMember, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            return JResult(IUserService.GetPageList(pageIndex, pageSize, name, phone, isMember, createdTimeStart, createdTimeEnd));
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Find(string ID)
        {
            var model = IUserService.Find(ID);
            return JResult(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(IUserService.Delete(ids));
        }
    }
}