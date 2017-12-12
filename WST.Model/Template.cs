
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

        /// <summary>
        /// 设计数据
        /// </summary>
        public string JsonData { get; set; }

        [Required]
        [MaxLength(32)]
        public string CategoryID { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
    }
}
