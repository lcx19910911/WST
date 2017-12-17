
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
    /// 秒杀
    /// </summary>
    [Table("MiaoSha")]
    public class MiaoSha : ActivityBase
    {

        /// <summary>
        /// 最低价
        /// </summary>
        public decimal LessPrice { get; set; }
    }
    

}

