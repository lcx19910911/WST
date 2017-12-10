
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
    /// 砍价
    /// </summary>
    [Table("KanJia")]
    public class KanJia : ActivityBase
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public string GoodsItemsJson { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal OldPrice { get; set; }

        /// <summary>
        /// 参加人数
        /// </summary>
        public int CountLimit { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public decimal LessPrice { get; set; }
        /// <summary>
        /// 单次砍价价格
        /// </summary>
        public decimal OncePrice { get; set; }

        /// <summary>
        /// 砍价间隔小时
        /// </summary>
        public int LimitHour { get; set; }

        /// <summary>
        /// 奖品数量
        /// </summary>
        public int PrizeCount { get; set; }

        /// <summary>
        /// 已领取数量  不需传值
        /// </summary>
        public int UsedCount { get; set; }
    }
    

}

