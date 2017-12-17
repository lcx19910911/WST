
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
        /// 次数限制
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
    }
    

}

