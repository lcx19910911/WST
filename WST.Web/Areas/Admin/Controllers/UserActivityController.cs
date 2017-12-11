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
    /// 参与记录
    /// </summary>
    public class UserActivityController : BaseAdminController
    {
        public IUserActivityService IUserActivityService;

        public UserActivityController(IUserActivityService _IUserActivityService)
        {
            this.IUserActivityService = _IUserActivityService;
        }
        public ViewResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="key"> 搜索项</param>
        /// <param name="value">搜索项</param>
        /// <returns></returns>
        public ActionResult GetPageList(int pageIndex, int pageSize,string targetId, string joinUserName,TargetCode? code)
        {
            return JResult(IUserActivityService.GetPageList(pageIndex, pageSize, targetId,"","", joinUserName, code));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(IUserActivityService.Delete(ids));
        }
    }
}