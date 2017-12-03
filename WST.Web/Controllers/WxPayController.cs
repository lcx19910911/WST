using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using WST.Web.Framework;
using WST.IService;
using WST.Core.Extensions;
using WST.Core.Web;
using WST.Core;
using WST.Model;
using WST.Core.Util;
using WxPayAPI;
using System.Web.Security;
using WST.Core.Helper;
using WST.Core.Extensions;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;

namespace WST.Web.Controllers
{
    public class WxPayController : BaseUserController
    {
        public IUserService IUserService;
        public IPayOrderService IPayOrderService;

        public WxPayController(IUserService _IUserService, IPayOrderService _IPayOrderService)
        {
            this.IUserService = _IUserService;
            this.IPayOrderService = _IPayOrderService;
        }
        public static string wxJsApiParam { get; set; } //H5调起JS API参数


        public ActionResult Pay(string orderId, string fee)
        {
            ViewBag.AppId = Params.WeixinAppId;
            string cacheToken = WxPayApi.GetCacheToken(Params.WeixinAppId, Params.WeixinAppSecret);
            ViewBag.TimeStamp = WxPayApi.GenerateTimeStamp();
            ViewBag.NonceStr = WxPayApi.GenerateNonceStr();
            ViewBag.Signature = WxPayApi.GetSignature(Params.SiteUrl, cacheToken, ViewBag.TimeStamp, ViewBag.NonceStr);
            ViewBag.isShowFooter = false;
            return View();
        }

        public ActionResult PaySuccess()
        {
            return View();
        }


        public ActionResult GetPayParms(decimal fee, string orderId, string remark)
        {
            var openid = IUserService.Find(LoginUser.ID).OpenID;
            //JSAPI支付预处理
            try
            {
                var timeStamp = WxPayApi.GenerateTimeStamp();
                var nonce_str = WxPayApi.GenerateNonceStr();
                var mch_id = Params.WeixinMID;
                var appid = Params.WeixinAppId;
                var notify_url = Params.SiteUrl + "/WxPay/WxNotify";
                var spbill_create_ip = this.IP;
                var trade_type = "JSAPI";
                var key = Params.WeixinPaySecret;
                string tmpStr = "appid=" + appid + "&body=" + remark + "&mch_id=" + mch_id +
                    "&nonce_str=" + nonce_str + "&notify_url=" + notify_url + "&openid=" + openid + "&out_trade_no=" + orderId + "&spbill_create_ip=" + spbill_create_ip
                    + "&total_fee=" + (int)(fee * 100M) + "&trade_type=" + trade_type + "&key=" + key;
                var paySign = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "MD5").ToUpper();


                string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                string xml = "<xml>";
                xml += "<appid>" + appid + "</appid>";
                xml += "<body>" + remark + "</body>";
                xml += "<mch_id>" + mch_id + "</mch_id>";
                xml += "<nonce_str>" + nonce_str + "</nonce_str>";
                xml += "<notify_url>" + notify_url + "</notify_url>";
                xml += "<openid>" + openid + "</openid>";
                xml += "<out_trade_no>" + orderId + "</out_trade_no>";
                xml += "<spbill_create_ip>" + spbill_create_ip + "</spbill_create_ip>";
                xml += "<total_fee>" + (int)(fee * 100M) + "</total_fee>";
                xml += "<trade_type>" + trade_type + "</trade_type>";
                xml += "<sign>" + paySign + "</sign>";
                xml += "</xml>";
                string v = PostWebRequests(url, xml);
                var prepay_id = v;
                WxPayData jsApiParam = new WxPayData();
                jsApiParam.SetValue("appId", appid);
                jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
                jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
                jsApiParam.SetValue("signType", "MD5");
                jsApiParam.SetValue("package", "prepay_id=" + prepay_id);
                jsApiParam.SetValue("paySign", jsApiParam.MakeSign());
                //appId=wxa7598339ca7ec3c6&nonceStr=0ee9ae19d31a42cfba3c792a4bc87a53&package=prepay_id=wx20171023092929508cf5fdb10182130345&paySign=005200F320EB1DA0B4F6EDEC4D4E2C96&signType=MD5&timeStamp=1508722170
                LogHelper.WriteDebug("result:" + jsApiParam.ToUrl());
                return JResult(jsApiParam.ToJson());
            }
            catch (Exception ex)
            {
                return JResult(Core.Code.ErrorCode.sys_fail, ex.Message);
            }
        }
        /// <summary>
        /// 获取prepay_id
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        public string PostWebRequests(string postUrl, string menuInfo)
        {
            string returnValue = string.Empty;
            try
            {
                byte[] byteData = Encoding.UTF8.GetBytes(menuInfo);
                Uri uri = new Uri(postUrl);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteData.Length;
                //定义Stream信息
                Stream stream = webReq.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                stream.Close();
                //获取返回信息
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                returnValue = streamReader.ReadToEnd();
                //关闭信息
                streamReader.Close();
                response.Close();
                stream.Close();

                LogHelper.WriteDebug("111:" + returnValue);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(returnValue);
                XmlNodeList list = doc.GetElementsByTagName("xml");
                XmlNode xn = list[0];
                string prepay_ids = xn.SelectSingleNode("//prepay_id").InnerText;
                return prepay_ids;
                //如果是二维码扫描，请返回下边的code_url，然后自己再更具内容生成二维码即可
                //string code_url = xn.SelectSingleNode("//prepay_id").InnerText;
                //return code_url;
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebug("获取错误:" + ex.Message);
                return "";
            }
        }
        public void WxNotify()
        {
            Notify notify = new Notify(this.HttpContext);
            WxPayData notifyData = notify.GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Response.Write(res.ToXml());
                Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            var model = IPayOrderService.Find(transaction_id);
            if (model == null || model.State != PayState.WaitPay)
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Response.Write(res.ToXml());
                Response.End();
            }

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                IPayOrderService.FailedPayOrder(transaction_id);
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Response.Write(res.ToXml());
                Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                var result = IPayOrderService.SuccessPayOrder(transaction_id, transaction_id);
                if (result.Result)
                {
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                }
                else
                {
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单修改失败");
                }
                Response.Write(res.ToXml());
                Response.End();
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}