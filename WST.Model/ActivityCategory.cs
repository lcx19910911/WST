﻿
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
    /// 活动分类
    /// </summary>
    [Table("ActivityCategory")]
    public class ActivityCategory : BaseEntity
    {
        /// <summary>
        /// 课程分类名称
        /// </summary>
        [Required(ErrorMessage = "课程分类名称不能为空")]
        [MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int? Sort { get; set; }

    }
}
