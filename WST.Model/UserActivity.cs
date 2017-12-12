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
    /// 用户参与记录
    /// </summary>
    [Table("UserActivity")]
    public class UserActivity : BaseEntity
    {
        [MaxLength(32)]
        public string JoinUserID { get; set; }
        [Required, MaxLength(32)]
        public string Openid { get; set; }

        public TargetCode Code { get; set; } = TargetCode.None;

        public string TargetID { get; set; }

        /// <summary>
        /// 是否中奖
        /// </summary>
        public bool IsPrize { get; set; } = false;

        /// <summary>
        /// 现价
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 中奖信息
        /// </summary>
        public string PrizeInfo { get; set; }
        
        [MaxLength(11)]
        public string Mobile { get; set; }
        [Required, MaxLength(32)]
        public string JoinUserName { get; set; }

        /// <summary>
        /// 为谁砍价
        /// </summary>
        [MaxLength(11)]
        public string TargetUserID { get; set; }
        /// <summary>
        /// 店家
        /// </summary>
        [Required, MaxLength(32)]
        public string ShopUserID { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUsedOnLine { get; set; } = false;

        /// <summary>
        /// 线下使用时间
        /// </summary>
        public System.DateTime? UsedTime { get; set; }

        /// <summary>
        /// 要求字段
        /// </summary>
        public string FiledItemJson { get; set; }
    }

}
