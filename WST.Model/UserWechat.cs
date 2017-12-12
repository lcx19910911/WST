
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WST.Model
{
    /// <summary>
    /// 用户公众号
    /// </summary>
    [Table("UserWechat")]
    public class UserWechat : BaseEntity
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Required(ErrorMessage = "用户不能为空")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 公众号名称
        /// </summary>
        [Required, MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [NotMapped]
        public string UserName { get; set; }

        /// <summary>
        /// 课程
        /// </summary>
        [Required(ErrorMessage = "微信appid")]
        public string AppId { get; set; }

        /// <summary>
        /// 课程
        /// </summary>
        [Required(ErrorMessage = "微信appsecrect")]
        public string AppSecrect { get; set; }
    }
}
