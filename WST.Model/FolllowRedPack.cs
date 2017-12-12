
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
    /// 关注送红包
    /// </summary>
    [Table("FolllowRedPack")]
    public class FolllowRedPack : BaseEntity
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Required(ErrorMessage = "用户不能为空")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [NotMapped]
        public string UserName { get; set; }

        /// <summary>
        /// 静态页面地址
        /// </summary>
        [MaxLength(256)]
        public string StaticHtmlUrl { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Required(ErrorMessage = "简介不能为空")]
        public string Description { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        [Required(ErrorMessage = "模板不能为空")]
        [MaxLength(32)]
        public string TemplateID { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        [NotMapped]
        public string TemplateName { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 单个红包金额
        /// </summary>
        public decimal OneRedPack { get; set; }
    }
}
