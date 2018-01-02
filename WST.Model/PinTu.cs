
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
    /// 拼图
    /// </summary>
    [Table("PinTu")]
    public class PinTu : ActivityBase
    {

        /// <summary>
        /// 拼图人数和价格json集合
        /// </summary>
        [MaxLength(256)]
        public string PinTuItemJson { get; set; }    

        /// <summary>
        /// 参加人数
        /// </summary>
        public int JoinCount { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal LessPrice { get; set; }

    }

    public class PinTuItem
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 拼图图片
        /// </summary>
        public string  Picture { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Amount { get; set; }


        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnNum { get; set; }
        /// <summary>
        /// 行数
        /// </summary>
        public int RowNum { get; set; }
    }


}

