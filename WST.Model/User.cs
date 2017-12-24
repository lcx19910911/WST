namespace WST.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 用户
    /// </summary>
    [Table("User")]
    public partial class User : BaseEntity
    {

        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码"), MaxLength(11)]
        public string Mobile { get; set; }


        [Display(Name = "密码"), MaxLength(32)]
        public string Password { get; set; }


        /// <summary>
        /// 请输入密码
        /// </summary>
        [Display(Name = "请输入密码")]
        [MaxLength(12), MinLength(6)]
        [NotMapped]
        public string NewPassword { get; set; }

        /// <summary>
        /// 再次输入密码
        /// </summary>
        [Display(Name = "再次输入密码")]
        [MaxLength(12), MinLength(6), Compare("NewPassword", ErrorMessage = "两次密码输入不一致")]
        [NotMapped]
        public string ConfirmPassword { get; set; }


        [MaxLength(32)]
        public string OpenID { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像"), MaxLength(512)]
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        [Display(Name = "国家"), MaxLength(32)]
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [Display(Name = "省份"), MaxLength(32)]
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "城市"), MaxLength(32)]
        public string City { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public SexCode Sex { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称"), MaxLength(32)]
        public string NickName { get; set; }

        /// <summary>
        /// 是否会员
        /// </summary>
        public bool IsMember { get; set; } = false;

        /// <summary>
        /// 身份证
        /// </summary>
        //public string IDCard { get; set; }

        /// <summary>
        /// 门店名
        /// </summary>
        //public string StoreName { get; set; }

        /// <summary>
        /// 顾问
        /// </summary>
        [Display(Name = "顾问"), MaxLength(32)]
        public string AdviserID { get; set; }

        [NotMapped]
        public string AdviserName { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balance { get; set; } = 0;
        /// <summary>
        /// 总充值
        /// </summary>
        public decimal TotalRecharge { get; set; } = 0;

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        public System.DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        public System.DateTime? EndTime { get; set; }
    }
}
