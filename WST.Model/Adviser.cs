namespace WST.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 顾问
    /// </summary>
    [Table("Adviser")]
    public partial class Adviser : BaseEntity
    {

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public SexCode Sex { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码"), MaxLength(32)]
        public string Mobile { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名"), MaxLength(32)]
        public string Name { get; set; }       
    }

}
