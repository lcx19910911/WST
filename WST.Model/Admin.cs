namespace WST.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 管理员
    /// </summary>
    [Table("Admin")]
    public partial class Admin : BaseEntity
    {

        [Display(Name = "账号"), MaxLength(32)]
        public string Account { get; set; }

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

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public SexCode Sex { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码"),MaxLength(32)]
        public string Mobile { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名"), MaxLength(32)]
        public string RealName { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Required(ErrorMessage = "角色不能为空")]
        [MaxLength(32)]
        public string RoleID { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [NotMapped]
        public string RoleName { get; set; }

        public bool IsSuperAdmin { get; set; } = false;

    }

}
