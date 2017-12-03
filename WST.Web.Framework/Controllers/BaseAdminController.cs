
using WST.Core;
using WST.Core.Model;
using WST.Web.Framework.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WST.Web.Framework
{
    [AdminLoginFilter]
    public class BaseAdminController : BaseController
    {
        private LoginUser _loginAdmin = null;

        public LoginUser LoginAdmin
        {
            get
            {

                return _loginAdmin != null ? _loginAdmin : LoginHelper.GetCurrentAdmin();
            }
            set
            {
                LoginHelper.CreateUser(value, Params.AdminCookieName);
            }
        }
    }
}