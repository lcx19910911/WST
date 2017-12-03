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
    /// 支付记录
    /// </summary>
    [Table("PayOrder")]
    public class PayOrder : BaseEntity
    {

        [Required, MaxLength(32)]
        public string NO { get; set; }
        [Required,MaxLength(32)]
        public string UserID { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        [MaxLength(32)]
        public string ThirdOrderID { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderCode Code { get; set; }

        public string TargetID { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Amount { get; set; }


        public PayCode Type { get; set; } = PayCode.WechatPay;
        [NotMapped]
        public string TypeStr { get; set; }

        public PayState State { get; set; } = PayState.WaitPay;
        [NotMapped]
        public string StateStr { get; set; }
        

        public string Remark { get; set; }
        /// <summary>
        /// 到账时间
        /// </summary>
        [Display(Name = "到账时间")]
        public System.DateTime? SuccessTime { get; set; }
        
    }

}
