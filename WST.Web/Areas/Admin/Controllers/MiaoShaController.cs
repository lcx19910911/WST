using WST.Core.Helper;
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
    /// 秒杀
    /// </summary>
    public class MiaoShaController : BaseAdminController
    {
        public IMiaoShaService IMiaoShaService;

        public MiaoShaController(IMiaoShaService _IMiaoShaService)
        {
            this.IMiaoShaService = _IMiaoShaService;
        }
        // GET: 
        public ActionResult Index()
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
        public ActionResult GetPageList(int pageIndex, int pageSize, string name,string userName)
        {
            return JResult(IMiaoShaService.GetPageList(pageIndex, pageSize,"", name, userName));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(IMiaoShaService.Delete(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string id)
        {
            return JResult(IMiaoShaService.Find(id));
        }
        /// <summary>
        /// 获取地区下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSelectItem()
        {
            return JResult(IMiaoShaService.GetSelectList());
        }
    }
}