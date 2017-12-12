
using WST.Core;
using WST.Core.Code;
using WST.Core.Extensions;
using WST.Core.Model;
using WST.Web.Framework.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WST.Web.Framework
{
    [ShopLoginFilter]
    public class BaseShopController : BaseController
    {


        private LoginUser _loginUser = null;

        public LoginUser LoginUser
        {
            get
            {

                return _loginUser != null ? _loginUser : LoginHelper.GetCurrentUser();
            }
            set {
                LoginHelper.CreateUser(value, Params.UserCookieName);
            }
        }
    }
}