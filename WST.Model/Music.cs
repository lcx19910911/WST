
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
    /// 背景音乐
    /// </summary>
    [Table("Music")]
    public class Music : BaseEntity
    {

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [MaxLength(32)]
        public string Name { get; set; }


        /// <summary>
        /// 音乐分类
        /// </summary>
        [Required(ErrorMessage = "音乐分类不能为空")]
        [MaxLength(32)]
        public string CategoryID { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
        /// <summary>
        /// 跳转链接
        /// </summary>
        [MaxLength(256)]
        public string Url { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } 


    }
}
