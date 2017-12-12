
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
    /// 轮播图
    /// </summary>
    [Table("Carousel")]
    public class Carousel : BaseEntity
    {

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [MaxLength(32)]
        public string Title { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        [MaxLength(256)]
        public string Url { get; set; }


        /// <summary>
        /// 图片
        /// </summary>
        [MaxLength(256)]
        public string Picture { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } 


    }
}
