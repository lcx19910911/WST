
using WST.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WST.Core.Extensions;

namespace WST.Core.Model
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        public LoginUser(User user)
        {
            this.ID = user.ID;
            this.Account = user.NickName;
            this.HeadImgUrl = user.HeadImgUrl;
            this.IsMember = user.IsMember;
            this.EndTime = user.EndTime;
            this.Openid = user.OpenID;
        }

        public LoginUser(Admin admin,string menuIDStr,string opreateSrt)
        {

            this.ID = admin.ID;
            this.Account = admin.Account;
            this.IsSuperAdmin = admin.IsSuperAdmin;
            this.MenuIDStr = menuIDStr;
            this.OpreateSrt = opreateSrt;
        }

        public LoginUser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string HeadImgUrl { get; set; }

        public DateTime? EndTime { get; set; }


        public string MenuIDStr { get; set; }

        public string OpreateSrt { get; set; }
        /// <summary>
        /// 权限值
        /// </summary>
        public bool IsMember { get; set; } = false;

        /// <summary>
        /// 权限值
        /// </summary>
        public bool IsSuperAdmin { get; set; } = false;
    }
}
