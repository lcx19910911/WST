
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
    /// 模板
    /// </summary>
    [Table("Template")]
    public class Template : BaseEntity
    {
        /// <summary>
        /// 课程名称
        /// </summary>
        [Required(ErrorMessage = "模板名称不能为空")]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required(ErrorMessage = "模板图片不能为空"), Display(Name = "模板图片"), MaxLength(256)]
        public string Picture { get; set; }


        /// <summary>
        /// 介绍文本集合
        /// </summary>
        [Required(ErrorMessage = "介绍不能为空")]
        [MaxLength(256)]
        public string Introduce { get; set; }


        /// <summary>
        /// 示例路径
        /// </summary>
        [Required(ErrorMessage = "示例路径不能为空")]
        [MaxLength(256)]
        public string DemoUrl { get; set; }


        /// <summary>
        /// 设计数据
        /// </summary>
        [MaxLength(32)]
        public string ClassNo { get; set; }
        /// <summary>
        /// 设计数据
        /// </summary>
        [NotMapped]
        public string RotueName { get; set; }

        [Required]
        [MaxLength(32)]
        public string CategoryID { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }


        [NotMapped]
        public string TemplateUrl { get; set; }
    }
}
