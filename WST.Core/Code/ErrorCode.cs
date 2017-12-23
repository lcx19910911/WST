using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WST.Core.Code
{
    public enum ErrorCode
    {
        #region 系统操作0-99之间
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功.")]
        sys_success = 0,

        /// <summary>
        /// 操作失败,请联系管理员
        /// </summary>
        [Description("服务器异常.")]
        sys_fail = 1,

        /// <summary>
        /// 参数值格式有误
        /// </summary>
        [Description("参数值格式有误.")]
        sys_param_format_error = 2,


        /// <summary>
        /// 授权码无效
        /// </summary>
        [Description("授权码无效.")]
        sys_token_invalid = 11,

        /// <summary>
        /// 用户角色权限不足
        /// </summary>
        [Description("用户角色权限不足.")]
        sys_user_role_error = 12,


        /// <summary>
        /// 微信openid不存在
        /// </summary>
        [Description("微信openid不存在.")]
        openid_no_exit = 13,

        /// <summary>
        /// 微信openid不存在
        /// </summary>
        [Description("微信openid已存在.")]
        openid_had_exit = 14,


        /// <summary>
        /// 已过验证时间
        /// </summary>
        [Description("已过验证时间.")]
        verification_time_out = 704,
        /// <summary>
        /// 账号已存在
        /// </summary>
        [Description("账号已存在.")]
        user_account_already_exist = 701,
        #endregion

        #region 数据库操作 100-199
        /// <summary>
        /// 无法找到对应主键Id
        /// </summary>
        [Description("无法找到对应主键Id.")]
        datadatabase_primarykey_not_found = 101,


        /// <summary>
        /// 两次密码输入不一样
        /// </summary>
        [Description("两次密码输入不一样")]
        user_password_notequal = 110,


        /// <summary>
        /// 用户不存在
        /// </summary>
        [Description("用户不存在.")]
        user_not_exit = 105,


        /// <summary>
        /// 旧密码输入错误
        /// </summary>
        [Description("旧密码输入错误")]
        user_password_nottrue = 109,



        /// <summary>
        /// 请先注册
        /// </summary>
        [Description("账号密码错误.")]
        password_not_true = 110,


        #endregion


        #region 共享的业务异常 200-299

        /// <summary>
        /// 请先注册
        /// </summary>
        [Description("请先注册.")]
        need_to_register = 200,

        /// <summary>
        /// 手机验证码错误
        /// </summary>
        [Description("手机验证码错误.")]
        phone_verificationCode_error = 201,


        /// </summary>
        [Description("名称已存在.")]
        system_name_already_exist = 202,
        /// </summary>
        [Description("编码已存在.")]
        system_key_already_exist = 203,
        
        /// </summary>
        [Description("手机号码已存在.")]
        system_phone_already_exist = 205,

        /// </summary>
        [Description("路径已存在.")]
        system_url_already_exist = 209,



        /// </summary>
        [Description("身份证号已存在.")]
        idcard_had_extis = 210,


        /// </summary>
        [Description("身份证号号码错误已存在.")]
        idcard_erroe = 211,



        /// </summary>
        [Description("路径已存在.")]
        route_name_already_exist = 212,

        /// </summary>
        [Description("路径下的模板代号已存在已存在.")]
        route_class_name_already_exist = 213,
        #endregion





        #region 用户



        /// <summary>
        /// 你已试用过
        /// </summary>
        [Description("你已试用过.")]
        user_had_try = 706,



        /// <summary>
        /// 第三方支付订单号已存在
        /// </summary>
        [Description("第三方支付订单号已存在.")]
       third_order_exit = 707,
        #endregion


    

        /// <summary>
        /// 不在活动时间内
        /// </summary>
        [Description("不在活动时间内.")]
        activity_time_out = 804,


        /// <summary>
        /// 已参加活动
        /// </summary>
        [Description("已参加活动.")]
        had_join_in = 805,
        /// <summary>
        /// 礼物已全部发放
        /// </summary>
        [Description("礼物已全部发放.")]
        prize_not_had = 806,


        /// <summary>
        /// 没到砍价间隔时间
        /// </summary>
        [Description("没到砍价间隔时间.")]
        time_limit_error = 807,
        /// <summary>
        /// 已到砍价次数限制
        /// </summary>
        [Description("已到砍价次数限制.")]
        count_limit_error = 808,
        /// <summary>
        /// 你已帮砍
        /// </summary>
        [Description("你已帮砍.")]
        had_kanjia = 809,
        /// <summary>
        /// 已到最低价，不能再砍了
        /// </summary>
        [Description("已到最低价，不能再砍了")]
        price_limit_error = 810,
        /// <summary>
        /// 开始时间小于当前时间
        /// </summary>
        [Description("开始时间小于当前时间.")]
        start_time_error = 811,
        /// <summary>
        /// 结束时间小于当前时间
        /// </summary>
        [Description("结束时间小于当前时间.")]
        end_time_error = 812,
    }
}
