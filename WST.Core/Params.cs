using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Security;
using WST.Core.Helper;

namespace WST.Core
{
    public class Params
    {
        /// <summary>
        /// 时间戳有效时间c
        /// </summary>
        public const int TimspanExpiredMinutes = 60;
        /// <summary>
        /// token失效时间
        /// </summary>
        public const int ExpiredDays = 7;

        public static readonly string SecretKey = "2343434xcxccxc";
        public static readonly string SiteUrl = CustomHelper.GetValue("Company_SiteUrl");
        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string UserCookieName = "website_user";
        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string UserTokenCookieName = "token";
        /// 登陆cookie
        /// </summary>
        public static readonly string AdminCookieName = "website_admin";


        /// 跟平台通信密钥
        /// </summary>
        public static readonly string WeixinAppSecret = ConfigHelper.GetValue("WeixinAppSecret");

        /// <summary>
        /// 平台地址
        /// </summary>
        public static readonly string WeixinAppId = ConfigHelper.GetValue("WeixinAppId");

        /// <summary>
        /// 微信支付商户id
        /// </summary>
        public static readonly string WeixinMID = ConfigHelper.GetValue("WeixinMID");

        /// <summary>
        /// 商户平台API密码
        /// </summary>
        public static readonly string WeixinPaySecret = ConfigHelper.GetValue("WeixinPaySecret");
        

        public static readonly string Cache_Prefix_Key = "wst_";


    }
}
