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
    /// 充值方案
    /// </summary>
    [Table("RechargePlan")]
    public class RechargePlan : BaseEntity
    {

        [Required,MaxLength(32)]
        public string Name { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [Required(ErrorMessage = "描述不能为空")]
        [MaxLength(32)]
        public string Introduce { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Money { get; set; }
        
        /// <summary>
        /// 天数
        /// </summary>
        public int Day { get; set; }
    }

}
