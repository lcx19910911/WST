using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using WST.Web.Framework;
using WST.IService;
using WST.Core.Extensions;
using WST.Core.Web;
using WST.Core;
using WST.Model;
using WST.Core.Code;
using WST.Web.Framework.Filters;
using WxPayAPI;
using WST.Core.Util;
using WST.Domain;

namespace WST.Web.Controllers
{
    /// <summary>
    /// 用户微信
    /// </summary>
    public class UserWechatController : BaseUserController
    {
        public IUserService IUserService;
        public IUserWechatService IUserWechatService;
  

        public UserWechatController(IUserService _IUserService, IUserWechatService _IUserWechatService
            )
        {
            this.IUserService = _IUserService;
            this.IUserWechatService = _IUserWechatService;
        }
           
        // GET: UserWechat
        public ActionResult Index(string id)
        {
            var model = IUserWechatService.Find(id);
            ViewBag.isShowFooter = false;
            return View(model);
        }

        ///// <summary>
        ///// 新增
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public JsonResult Add(Coach entity)
        //{
        //    ModelState.Remove("ID");
        //    ModelState.Remove("CreatedTime");
        //    ModelState.Remove("UpdatedTime");
        //    ModelState.Remove("IsDelete");
        //    if (ModelState.IsValid)
        //    {
        //        var result = ICoachService.AddCoach(entity);
        //        return JResult(result);
        //    }
        //    else
        //    {
        //        return ParamsErrorJResult(ModelState);
        //    }
        //}

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public JsonResult Update(Coach entity)
        //{
        //    ModelState.Remove("CreatedTime");
        //    ModelState.Remove("UpdatedTime");
        //    ModelState.Remove("IsDelete");
        //    if (ModelState.IsValid)
        //    {
        //        var result = ICoachService.UpdateCoach(entity);
        //        return JResult(result);
        //    }
        //    else
        //    {
        //        return ParamsErrorJResult(ModelState);
        //    }
        //}

    }
}