using WST.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WST.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        // GET: Admin/Default
        public ActionResult Index()
        {
            return View();
        }
    }
}