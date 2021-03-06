﻿using System;
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
        [Description("支付宝")]
        Alipay = 0,

        [Description("微信")]
        WechatPay = 1,

        [Description("余额")]
        Recharge =2
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

    public enum RedPackCode
    {
        [Description("等待领取")]
        Wait = 0,
        [Description("发送红包失败")]
        Failed = 1,
        [Description("领取成功")]
        Success = 2,
        [Description("过期退还")]
        Return = 3,   
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
        /// 音乐分类
        /// </summary>
        [Description("音乐分类")]
        MusicCategory = 2,
        /// <summary>
        /// 特效
        /// </summary>
        [Description("特效")]
        Function = 3,
    }


   

    public enum OrderCode
    {
        /// <summary>
        /// 套餐
        /// </summary>
        [Description("套餐")]
        Time = 0,
        /// <summary>
        /// 充值
        /// </summary>
        [Description("充值")]
        Recharge = 1,

        /// <summary>
        /// 活动
        /// </summary>
        [Description("活动")]
        Activity = 2,
    }



    /// <summary>
    /// 活动类型
    /// </summary>
    public enum TargetCode
    {
        None = 0,
        /// <summary>
        /// 拼团
        /// </summary>
        [Description("拼团")]
        Pintuan = 1,

        /// <summary>
        /// 秒杀
        /// </summary>
        [Description("秒杀")]
        Miaosha = 2,
        /// <summary>
        /// 砍价
        /// </summary>
        [Description("砍价")]
        Kanjia = 3,
        /// <summary>
        /// 拼图
        /// </summary>
        [Description("拼图")]
        Pintu = 4,
        /// <summary>
        /// 助力
        /// </summary>
        [Description("助力")]
        Zhuli = 5,
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
    /// 客服消息类型
    /// </summary>
    public enum Enum_WXServiceMsg_Type
    {
        文本消息 = 1,
        图文消息 = 2
    }

}
