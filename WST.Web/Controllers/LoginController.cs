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
using WST.Core.Util;
using System.Security.Cryptography;
using System.Text;
using WST.Core.Helper;
using WST.Core.Model;
using WST.Core.Code;
using System.IO;
using System.Xml;
using WST.Core.Mp;

namespace WST.Web.Controllers
{
    public class LoginController : BaseUserController
    {

        public IUserService IUserService;
        public IAdminService IAdminService;
        public IMpAutoReplyService IMpAutoReplyService;


        public LoginController(IUserService _IUserService, IAdminService _IAdminService, IMpAutoReplyService _IMpAutoReplyService)
        {
            this.IUserService = _IUserService;
            this.IAdminService = _IAdminService;
            this.IMpAutoReplyService = _IMpAutoReplyService;
        }

        // GET: Login
        public ActionResult Shop()
        {
            
            return View();
        }

        public ActionResult ShopLogin(string account, string password)
        {
            var result = IUserService.Login(account, password);
            if (result.Success)
            {

                if (result.Result == null)
                    return JResult(new WebResult<bool> { Code = ErrorCode.password_not_true, Result = false, Append = "账号密码错误" });
                else
                {
                    if (result.Result.IsDelete)
                    {
                        return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "该账户已被删除" });
                    }
                    else
                    {
                        this.LoginUser = new Core.Model.LoginUser(result.Result);
                    }
                }

            }
            return JResult(result);
        }

        // GET: Login
        public ActionResult Index()
        {

            if (Request.UserAgent.IsNotNullOrEmpty() && Request.UserAgent.ToLower().Contains("micromessenger"))
            {
                WeixinLoginAction();
            }

            return View();
        }

        public ActionResult ResetPassword()
        {
            var user = IUserService.Find(LoginUser.ID);
            if (user == null && user.IsDelete)
            {
                return Forbidden();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult ResetPassword(string Password,string Mobile, string Code)
        {
            var user = IUserService.Find(LoginUser.ID);
            if (user == null && user.IsDelete)
            {
                return DataErorrJResult();
            }
            if (Code != CacheHelper.Get<string>("mobile_" + Mobile))
            {
                return JResult(Core.Code.ErrorCode.phone_verificationCode_error, "");
            }
            user.Password = Core.Util.CryptoHelper.MD5_Encrypt(Password);
            var result = IUserService.Update(user);
            if (result > 0)
            {
                //刷新时间
                LoginHelper.CreateUser(new LoginUser(user), Params.UserCookieName);
            }
            return JResult(result);
        }

        public ActionResult DefaultUser()
        {
            //var model = new Model.User()
            //{
            //    ID="11111",
            //    NickName = "lcsdsdx",
            //    OpenID = "343fdfdfdzzsdsdscdsc",
            //    HeadImgUrl = "http://wx.qlogo.cn/mmopen/vi_32/XayCtyvRZwdrXnPgD1WtDL2TY4Ztz3tHLlT8oah80Lec1LnmyLK4dFhxL5tNxl5pFeDlRg8fQwaoFHmA9U692w/0",
            //    Country = "中国",
            //    Province = "福建",
            //    City = "福州",
            //    Sex = Model.SexCode.Man,
            //    IsMember = false,
            //    //EndTime=DateTime.Now.AddDays(1)
            //};
            //var result = IUserService.Add(model);
            //lcx oZe9g0tkskZx51DFih_hKm_GIYS0
            //lzy  oZe9g0hGp6RNI67KQ1WahfbO0e4Q
            //shop  oZe9g0pyZyvXx2MCC1MS9bcx-aQM
            var user = IUserService.FindByOpenId("oZe9g0hGp6RNI67KQ1WahfbO0e4Q");
            if (user != null)
            {
                //user.IsMember = false;
                //user.EndTime = DateTime.Now.AddMinutes(2);
                this.LoginUser = new Core.Model.LoginUser(user);
            }
                return RedirectToAction("Index", "Home");
        }
        
        public void WeixinLoginAction()
        {
            string code = this.Request.QueryString["code"];

            if (this.LoginUser==null)
            {
                //请求回来
                if (!string.IsNullOrEmpty(code))
                {
                    var url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + Params.WeixinAppId + "&secret=" + Params.WeixinAppSecret + "&code=" + code + "&grant_type=authorization_code";         
                    string responseResult = WebHelper.GetPage(url);
                    string redirecturl = Request["redirecturl"];
                    if (redirecturl.IsNullOrEmpty())
                    {
                        redirecturl = Params.SiteUrl;
                    }
                    if (responseResult.Contains("access_token"))
                    {
                        JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;

                        var access_token = obj2["access_token"].ToString();
                        string openId = obj2["openid"].ToString();
                        var user = IUserService.FindByOpenId(openId);
                        if (user == null)
                        {
                            string userResponseResult = WebHelper.GetPage("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openId + "&lang=zh_CN");
                            JObject obj3 = JsonConvert.DeserializeObject(userResponseResult) as JObject;
                            if (obj3 != null)
                            {
                                var model = new Model.User()
                                {
                                    NickName = obj3["nickname"].ToString(),
                                    OpenID = obj3["openid"].ToString(),
                                    Sex = (Model.SexCode)obj3["sex"].GetInt(),
                                    Province = obj3["province"].ToString(),
                                    City = obj3["city"].ToString(),
                                    Country = obj3["country"].ToString(),
                                    HeadImgUrl = obj3["headimgurl"].ToString(),
                                    IsMember = false,
                                    //EndTime=DateTime.Now.AddDays(1)
                                };
                                var result = IUserService.Add(model);
                                if (result > 0)
                                {
                                    user = IUserService.FindByOpenId(model.OpenID);
                                    this.LoginUser = new Core.Model.LoginUser(user);
                                    this.Response.Redirect(redirecturl);
                                }else
                                {
                                    this.Response.Redirect("/base/Forbidden");
                                }
                            }
                            else
                            {
                                this.Response.Redirect("/base/_404");
                            }
                        }
                        else
                        {
                                this.LoginUser = new Core.Model.LoginUser(user);
                                this.Response.Redirect(redirecturl);
                        }

                    }
                    else
                    {
                        this.Response.Redirect("/base/_404");
                    }
                }
                else if (!string.IsNullOrEmpty(this.Request.QueryString["state"]))
                {
                    this.Response.Redirect(Params.SiteUrl);
                }
                else
                {
                    string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + Params.WeixinAppId + "&redirect_uri=" + this.Request.Url.ToString() + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
                    this.Response.Redirect(url);
                }

            }

        }
        /// <summary>
        /// 获取post请求数据
        /// </summary>
        /// <returns></returns>
        private string PostInput()
        {
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            return Encoding.UTF8.GetString(b);
        }

        //用于申请“成为开发者”时向微信发送验证信息。
        public void Valid()
        {
            if (this.HttpContext.Request.HttpMethod == "GET")
            {
                string echoStr = Request.QueryString["echoStr"];
                if (string.IsNullOrEmpty(echoStr))
                {
                    return;
                }
                string signature = Request.QueryString["signature"];
                string timestamp = Request.QueryString["timestamp"];
                string nonce = Request.QueryString["nonce"];
                if (CheckSignature(signature, timestamp, nonce))
                {
                    Response.Write(echoStr);
                    Response.End();
                }
            }
            else if (Request.HttpMethod == "POST")
            {
                string weixin = "";
                                weixin = PostInput();//获取xml数据
                                 if (!string.IsNullOrEmpty(weixin))
                                   {
                                        ResponseMsg(weixin);////调用消息适配器
                                     }
            }
        }


        #region 消息类型适配器
        private RequestMsg GetExmlMsg(XmlElement root)
        {
            RequestMsg xmlMsg = new RequestMsg()
            {
                FromUserName = root.SelectSingleNode("FromUserName").InnerText,
                ToUserName = root.SelectSingleNode("ToUserName").InnerText,
                CreateTime = root.SelectSingleNode("CreateTime").InnerText,
                MsgType = root.SelectSingleNode("MsgType").InnerText,
            };
            if (xmlMsg.MsgType.Trim().ToLower() == "text")
            {
                xmlMsg.Content = root.SelectSingleNode("Content").InnerText;
            }
            else if (xmlMsg.MsgType.Trim().ToLower() == "event")
            {
                xmlMsg.EventName = root.SelectSingleNode("Event").InnerText;
            }
            return xmlMsg;
        }

        private void ResponseMsg(string weixin)// 服务器响应微信请求
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(weixin);//读取xml字符串
            XmlElement root = doc.DocumentElement;
            RequestMsg xmlMsg = GetExmlMsg(root);
            //XmlNode MsgType = root.SelectSingleNode("MsgType");
            //string messageType = MsgType.InnerText;
            string messageType = xmlMsg.MsgType;//获取收到的消息类型。文本(text)，图片(image)，语音等。


            try
            {

                switch (messageType)
                {
                    //当消息为文本时
                    case "text":
                        textCase(xmlMsg);
                        break;
                    case "event":
                        if (!string.IsNullOrEmpty(xmlMsg.EventName) && xmlMsg.EventName.Trim() == "subscribe")
                        {
                            //刚关注时的时间，用于欢迎词  
                            int nowtime = ConvertDateTimeInt(DateTime.Now);
                            string msg = "Hi~欢迎关注沃商通\r\n目前有超过百家商家正在使用的营销神器\r\n回复：1，了解沃商通\r\n回复：2，联系客服\r\n回复：3，了解收费";
                            string resxml = "<xml><ToUserName><![CDATA[" + xmlMsg.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + xmlMsg.ToUserName + "]]></FromUserName><CreateTime>" + nowtime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + msg + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                            Response.Write(resxml);
                        }
                        break;
                    case "image":
                        break;
                    case "voice":
                        break;
                    case "vedio":
                        break;
                    case "location":
                        break;
                    case "link":
                        break;
                    default:
                        break;
                }
                Response.End();
            }
            catch (Exception)
            {

            }
        }
        #endregion

        private string getText(RequestMsg xmlMsg)
        {
            string con = xmlMsg.Content.Trim();

            var content = IMpAutoReplyService.Find(x => !x.IsDelete && x.Keyword == con);
            if (content != null)
            {
                return content.Details;
            }
            else
                return "";
        }


        #region 操作文本消息 + void textCase(XmlElement root)
        private void textCase(RequestMsg xmlMsg)
        {
            int nowtime = ConvertDateTimeInt(DateTime.Now);
            string msg = "";
            msg = getText(xmlMsg);
            if (msg.IsNotNullOrEmpty())
            {
                string resxml = "<xml><ToUserName><![CDATA[" + xmlMsg.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + xmlMsg.ToUserName + "]]></FromUserName><CreateTime>" + nowtime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + msg + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                Response.Write(resxml);
            }
        }
        #endregion
        #region 将datetime.now 转换为 int类型的秒
        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        private int converDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion


        public bool CheckSignature(string signature, string timestamp, string nonce)
        {
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce))
            {
                return false;
            }
            //这个变量要与网页里面填写的接口配置信息中填写的Token一致
            string Token = ConfigHelper.GetValue("Token");;
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);//排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = SHA1_Hash(tmpStr);//对该字符串进行sha1加密
            tmpStr = tmpStr.ToLower();

            //获得加密后的字符串可与signature对比
            //通过检验signature对请求进行校验，若正确，则原样返回echostr参数内容，接入生效，否则接入失败
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 字符串转换SHA1
        /// </summary>
        /// <param name="str_sha1_in"></param>
        /// <returns></returns>
        public  string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "");
            return str_sha1_out;
        }
        public string GetApplicationPath()
        {
            string applicationPath = "/";
            if (Request.RequestContext != null)
            {
                try
                {
                    applicationPath = Request.ApplicationPath;
                }
                catch
                {
                    applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                }
            }
            if (applicationPath == "/")
            {
                return string.Empty;
            }
            return applicationPath.ToLower(CultureInfo.InvariantCulture);
        }



        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Quit()
        {
            this.LoginUser = null;
            return Redirect(Params.SiteUrl+"/login/Index");
        }
    }
}