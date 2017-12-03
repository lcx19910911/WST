using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WST.Model
{
   

    /// <summary>
    /// 后台用户类型
    /// </summary>
    public enum SexCode
    {
        [Description("未知")]
        UnKnow = 0,

        [Description("男")]
        Man = 1,

        [Description("女")]
        Women = 2
    }


    public enum PayCode
    {
        [Description("未知")]
        Alipay = 0,

        [Description("未知")]
        WechatPay = 1,

        [Description("余额")]
        Recharge = 2
    }

    public enum CourseCode
    {
        [Description("免费团课")]
        Free = 0,

        [Description("收费团课")]
        League = 1,
        [Description("私教")]
        Coach = 2,
    }
    

    public enum PayState
    {

        [Description("等待支付")]
        WaitPay = 0,

        [Description("支付成功")]
        Success = 1,
        [Description("支付失败")]
        Failed = 2,
        [Description("退款")]
        Return = 3,

        [Description("已拿走")]
        Taken =4,
    }
    
    /// <summary>
    /// 字典分组
    /// </summary>
    public enum GroupCode
    {
        None = 0,
        /// <summary>
        /// 地区
        /// </summary>
        [Description("地区")]
        Area = 1,

        /// <summary>
        /// 来源
        /// </summary>
        [Description("来源")]
        Source = 2,
    }


   

    public enum OrderCode
    {
        /// <summary>
        /// 会员卡
        /// </summary>
        [Description("会员卡")]
        Time = 0,
        /// <summary>
        /// 充值
        /// </summary>
        [Description("充值")]
        Recharge = 1,

        /// <summary>
        /// 商品消费
        /// </summary>
        [Description("商品消费")]
        Activity = 2,
    }


    /// <summary>
    /// 回复类型
    /// </summary>
    public enum Enum_AutoReplay_Type
    {
        关注 = 1,
        默认 = 2,
        关键字 = 3
    }

    /// <summary>
    /// 素材类别
    /// </summary>
    public enum Enum_Material_Type
    {
        文本 = 1,
        图片 = 2
    }

    /// <summary>
    /// 页面按钮类型
    /// </summary>
    public enum Enum_Button_Type
    {
        点击事件 = 1,
        跳转页面 = 2
    }
    /// <summary>
    /// 图文素材类型枚举
    /// </summary>
    public enum Enum_MpNews_Type
    {
        单图文素材 = 1,
        多图文素材 = 2
    }
}
