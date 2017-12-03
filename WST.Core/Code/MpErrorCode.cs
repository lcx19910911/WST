using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WST.Core.Code
{
    public static class MpErrorCode
    {
        public static string GetErrmsg(string errcode)
        {
            switch (errcode)
            {
                case "-1":
                    return "系统繁忙，此时请开发者稍候再试";
                case "0":
                    return "请求成功";
                case "48001":
                    return "api功能未授权，请确认公众号已获得该接口，可以在公众平台官网-开发者中心页中查看接口权限";
                case "50001":
                    return "用户未授权该api";
                case "40017":
                    return "不合法的按钮个数";
                case "41001":
                    return "获取access_token失败，请稍候再试";
                default:
                    return "未知错误";
            }
        }
    }
}
