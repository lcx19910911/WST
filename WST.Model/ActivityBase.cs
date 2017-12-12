
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
    /// 活动基础
    /// </summary>
    [Table("ActivityBase")]
    public class ActivityBase : BaseEntity
    { /// <summary>
      /// 用户  不用传值
      /// </summary>
        [Required(ErrorMessage = "用户不能为空")]
        [MaxLength(32)]
        public string UserID { get; set; }
        /// <summary>
        /// 用户  不用传值
        /// </summary>
        [NotMapped]
        public string UserName { get; set; }
        /// <summary>
        ///  不用传值
        /// </summary>
        [Display(Name = "静态路径"), MaxLength(256)]
        public string StaticHtmlUrl { get; set; }


        [Display(Name = "活动名称"), MaxLength(32)]
        public string Name { get; set; }

        [Required(ErrorMessage = "活动图片不能为空"),Display(Name = "活动图片"), MaxLength(256)]
        public string Picture { get; set; }

        /// <summary>
        /// 活动是否完成  不用传值
        /// </summary>
        public bool IsCompalte { get; set; } = false;

        /// <summary>
        /// 是否需要支付  不用传值
        /// </summary>
        public bool IsNeedPay { get; set; } = true;

        /// <summary>
        /// 支付数量 如拼团，需传值
        /// </summary>
        public decimal Amount { get; set; } = 0;

        /// <summary>
        /// 是否需要报名 不用传值
        /// </summary>
        public bool IsNeedReport { get; set; } = true;

        /// <summary>
        /// 机构介绍文本集合
        /// </summary>
        [Required(ErrorMessage = "介绍不能为空")]
        [MaxLength(10240)]
        public string IntroduceTxtJson { get; set; }

        /// <summary>
        /// 商家介绍图片集合
        /// </summary>
        [MaxLength(1024)]
        public string IntroducePicturesJson { get; set; }


        /// <summary>
        /// 商家介绍视频集合
        /// </summary>
        [MaxLength(1024)]
        public string IntroduceVediosJson { get; set; }

        /// <summary>
        /// 联系
        /// </summary>
        [Required(ErrorMessage = "联系不能为空")]
        [MaxLength(1024)]
        public string Connact { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>
        [Required(ErrorMessage = "联系电话不能为空")]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 规则
        /// </summary>
        [Required(ErrorMessage = "规则不能为空")]
        [MaxLength(1024)]
        public string Rules { get; set; }

        /// <summary>
        /// 背景音乐
        /// </summary>
        [MaxLength(128)]
        public string MusicUrl { get; set; }

        /// <summary>
        /// 特效名称
        /// </summary>
        [MaxLength(32)]
        public string FunctionName { get; set; }

        /// <summary>
        /// 特效方向
        /// </summary>
        [MaxLength(32)]
        public string Direction { get; set; }

        /// <summary>
        /// 分享次数 不用传值
        /// </summary>
        public int ShareCount { get; set; } = 0;
        /// <summary>
        /// 报名人数 不用传值
        /// </summary>
        public int ReportCount { get; set; } = 0;
        /// <summary>
        /// 查看次数 不用传值
        /// </summary>
        public int ClickCount { get; set; } = 0;

        /// <summary>
        /// 悲观并发 不用传值
        /// </summary>
        [Timestamp]
        public byte[] TimeStamp { get; set; }

        /// <summary>
        /// 要求字段
        /// </summary>
        [MaxLength(512)]
        public string FiledItemJson { get; set; }
    }

    /// <summary>
    /// 字段要求
    /// </summary>
    public class FiledItem
    {
        [Display(Name = "商品名称"), MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// 商品类
    /// </summary>
    public class TypeContentItem
    {
        //[Display(Name = "商品名称"), MaxLength(32)]
        //public string Name { get; set; }

        ///// <summary>
        ///// 排序
        ///// </summary>
        //public int Sort { get; set; }

        //[Display(Name = "商品图片集合"), MaxLength(1024)]
        //public string Pictures { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Content { get; set; }
    }
}
