
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
    /// 案例
    /// </summary>
    [Table("Demo")]
    public class Demo:BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述"),MaxLength(128)]
        public string Description { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        public string Content { get; set; }

        /// <summary>
        /// 图片地址集合 ，隔开
        /// </summary>
        [MaxLength(256)]
        [Display(Name = "图片地址集合 ，隔开")]
        public string Picture { get; set; }
    }
}
