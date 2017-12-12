
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
    /// 用户参与关注送红包
    /// </summary>
    [Table("FolllowRedPackUserJoin")]
    public class FolllowRedPackUserJoin : BaseEntity
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Required(ErrorMessage = "用户不能为空")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [NotMapped]
        public string UserName { get; set; }

        /// <summary>
        /// 扫描送红包活动
        /// </summary>
        [Required(ErrorMessage = "扫描送红包活动")]
        [MaxLength(32)]
        public string FolllowRedPackID { get; set; }

        /// <summary>
        /// 扫描送红包活动
        /// </summary>
        [NotMapped]
        public string FolllowRedPackName { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 微信转账订单id
        /// </summary>
        public string ThirdOrderNO { get; set; }

        /// <summary>
        /// 红包状态
        /// </summary>
        public RedPackCode Code { get; set; }

        /// <summary>
        /// 领取红包时间
        /// </summary>
        public DateTime? ReviceTime { get; set; }
    }
}
