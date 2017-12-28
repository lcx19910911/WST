using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WST.Model
{

    /// <summary>
    /// 自动回复
    /// </summary>
    [Table("MpAutoReply")]
    public class MpAutoReply:BaseEntity
	{
		/// <summary>
        /// 回复类型
        /// </summary>
        [Display(Name = "回复类型")]
        [Required(ErrorMessage = "请输入回复类型")]
        public Enum_AutoReplay_Type AutoReplyType { get; set; }
		/// <summary>
        /// 素材类别
        /// </summary>
        [Display(Name = "素材类别")]
        public Enum_Material_Type MaterialType { get; set; }
		/// <summary>
        /// 关键字
        /// </summary>
        [Display(Name = "关键字")]
        [MaxLength(50)]
        public string Keyword{ get; set; }
		/// <summary>
        /// 是否全匹配
        /// </summary>
        [Display(Name = "是否全匹配")]
        public bool PerfectMatch{ get; set; }

        /// <summary>
        /// 文本信息
        /// </summary>
        [Display(Name = "文本信息")]
        [MaxLength(500)]
        public string Details { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [Display(Name = "图片路径")]
        [MaxLength(253)]
        public string FilePath { get; set; }
        /// <summary>
        /// 图片MediaId
        /// </summary>
        [Display(Name = "图片MediaId")]
        [MaxLength(50)]
        public string MediaId { get; set; }
    }
}