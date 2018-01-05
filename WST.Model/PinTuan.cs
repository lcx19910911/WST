
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
    /// 拼团
    /// </summary>
    [Table("PinTuan")]
    public class PinTuan : ActivityBase
    {

        /// <summary>
        /// 拼团人数和价格json集合
        /// </summary>
        [MaxLength(2560)]
        public string PinTuanItemJson { get; set; }

        /// <summary>
        /// 拼团描述
        /// </summary>
        [MaxLength(1024)]
        public string PinTuanItemInfo { get; set; }
        

        /// <summary>
        /// 参加人数
        /// </summary>
        public int JoinCount { get; set; }

    }

    public class PinTuanItem
    {
        /// <summary>
        /// 人数数量
        /// </summary>
        public int Count { get; set; }

        public decimal Amount { get; set; }
    }


}

