using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WST.Model
{
    /// <summary>
    /// 自定义菜单
    /// </summary>
    [Table("MpMenu")]
    public class MpMenu : BaseEntity
    {
        /// <summary>
        /// 父级菜单ID
        /// </summary>
        [Display(Name = "父级菜单ID")]
        [Required(ErrorMessage = "请输入父级菜单ID")]
        public string ParentID { get; set; }

        [NotMapped]
        public string ParentName { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Display(Name = "菜单名称")]
        [Required(ErrorMessage = "请输入菜单名称")]
        [MaxLength(4)]
        public string Name { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        [Display(Name = "回复内容")]
        [MaxLength(200)]
        public string ReplyText { get; set; }

        /// <summary>
        /// key类型   该字段标示回复文本类型
        /// </summary>
        [Display(Name = "key类型")]
        public Enum_WXServiceMsg_Type KeyType { get; set; }

        /// <summary>
        /// 触发类型
        /// </summary>
        [Display(Name = "触发类型")]
        public Enum_Button_Type EventType { get; set; }
        /// <summary>
        /// 事件Key值
        /// </summary>
        [Display(Name = "事件Key值")]
        [MaxLength(50)]
        public string EventKey { get; set; }
        /// <summary>
        /// 跳转URL
        /// </summary>
        [Display(Name = "跳转URL")]
        [MaxLength(100)]
        public string LinkUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入排序")]
        public int Sort { get; set; }

        /// <summary>
        /// 菜单的子级菜单
        /// </summary>
        [NotMapped]
        public List<MpMenu> ChildrenList { get; set; }
    }
}