﻿using WST.Core.Helper;
using WST.Core.Extensions;
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
    /// 管理员管理
    /// </summary>
    public class AdminController : BaseAdminController
    {
        public IAdminService IAdminService;
        public IUserService IUserService;

        public AdminController(IAdminService _IAdminService, IUserService _IUserService
        )
        {
            this.IAdminService = _IAdminService;
            this.IUserService = _IUserService;
        }
        // GET: 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
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
            return JResult(IAdminService.GetPageList(pageIndex, pageSize, name, mobile));
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(WST.Model.Admin entity)
        {
            ModelState.Remove("ID");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("Type");
            if (ModelState.IsValid)
            {
                if (IAdminService.IsExits(x => x.Account == entity.Account))
                {
                    return JResult(Core.Code.ErrorCode.user_account_already_exist);
                }
                entity.Password = CryptoHelper.MD5_Encrypt(entity.ConfirmPassword);
                entity.CreatedTime = entity.UpdatedTime = DateTime.Now;
                var result = IAdminService.Add(entity);
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
        public ActionResult Update(WST.Model.Admin entity)
        {
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Password");
            ModelState.Remove("Type");
            if (ModelState.IsValid)
            {
                var model = IAdminService.Find(entity.ID);
                if (model == null || (model != null && model.IsDelete))
                {
                    return DataErorrJResult();
                }
                
                if (IAdminService.IsExits(x => x.Account == entity.Account  && x.ID != entity.ID))
                {
                    return JResult(Core.Code.ErrorCode.system_name_already_exist, "");
                }

                model.RealName = entity.RealName;
                model.Mobile = entity.Mobile;
                model.RoleID = entity.RoleID;
                model.Sex = entity.Sex;
                var result = IAdminService.Update(model);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }
        public ActionResult ChangePassword(string oldPassword, string newPassword, string cfmPassword, string id)
        {
            return JResult(IAdminService.ChangePassword(oldPassword, newPassword, cfmPassword, id));
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(IAdminService.Delete(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string id)
        {
            return JResult(IAdminService.Find(id));
        }
    }
}