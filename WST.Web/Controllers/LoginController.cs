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

namespace WST.Web.Controllers
{
    public class LoginController : BaseUserController
    {

        public IUserService IUserService;
        public IAdminService IAdminService;


        public LoginController(IUserService _IUserService, IAdminService _IAdminService)
        {
            this.IUserService = _IUserService;
            this.IAdminService = _IAdminService;
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


        public ActionResult DefaultUser()
        {

            //lcx ojLuqwikV8T-nCd2VMAihJEqSOzw
            //lzy ojLuqwqwSS9xPpuHj-ZsHv6QDQSQ
            //shop ojLuqwhYFF5ELJviQmmrJrY2fbTY
            var user = IUserService.FindByOpenId("ojLuqwqwSS9xPpuHj-ZsHv6QDQSQ");
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
                                    ID = Guid.NewGuid().ToString("N"),
                                    NickName = obj3["nickname"].ToString(),
                                    OpenID = obj3["openid"].ToString(),
                                    Sex = (Model.SexCode)obj3["sex"].GetInt(),
                                    Province = obj3["province"].ToString(),
                                    City = obj3["city"].ToString(),
                                    Country = obj3["country"].ToString(),
                                    HeadImgUrl = obj3["headimgurl"].ToString(),
                                    IsMember = false,
                                    EndTime=DateTime.Now.AddDays(1)
                                };
                                IUserService.Add(model);
                                this.LoginUser = new Core.Model.LoginUser(model);
                                this.Response.Redirect(redirecturl);
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


        //用于申请“成为开发者”时向微信发送验证信息。
        public void Valid()
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
        public  bool CheckSignature(string signature, string timestamp, string nonce)
        {
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce))
            {
                return false;
            }
            //这个变量要与网页里面填写的接口配置信息中填写的Token一致
            string Token = "Eioa5C5oj3S32qhH";
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