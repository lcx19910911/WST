using WST.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WST.Core.Extensions;
using WST.IService;

namespace WST.Web.Controllers
{
    public class HomeController : BaseUserController
    {
        public IUserService IUserService;
        public ITemplateService ITemplateService;
        public ITemplateCategoryService ITemplateCategoryService;
        public ICarouselService ICarouselService;
        public HomeController(IUserService _IUserService, ITemplateService _ITemplateService, ITemplateCategoryService _ITemplateCategoryService, ICarouselService _ICarouselService)
        {
            this.IUserService = _IUserService;
            this.ITemplateService = _ITemplateService;
            this.ITemplateCategoryService = _ITemplateCategoryService;
            this.ICarouselService = _ICarouselService;
        }
        // GET: Home
        public ActionResult Index(string id)
        {
            var userModel = IUserService.Find(LoginUser.ID);
            if ((userModel.IsMember && !LoginUser.IsMember) || (userModel.EndTime < DateTime.Now && LoginUser.IsMember))
            {
                this.LoginUser = new Core.Model.LoginUser(userModel);
            }
            ViewBag.CarouseList = ICarouselService.GetList(x=>!x.IsDelete).OrderByDescending(x=>x.Sort).ToList();
            var tempelateList = ITemplateService.GetList(x=>!x.IsDelete);
            var categoryIdList = tempelateList.Select(x => x.CategoryID).Distinct().ToList();
            var dic = ITemplateCategoryService.GetDic(x => categoryIdList.Contains(x.ID) && !x.IsDelete);
            tempelateList.ForEach(x =>
            {
                if(dic.ContainsKey(x.CategoryID))
                {
                    x.RotueName = dic[x.CategoryID].RouteName;
                    x.TemplateUrl = $"/Template/{dic[x.CategoryID].RouteName}/{x.ClassNo}";
                }
            });
            return View(tempelateList);
        }


        public ActionResult New(string id)
        {
            var userModel = IUserService.Find(LoginUser.ID);
            if ((userModel.IsMember && !LoginUser.IsMember) || (userModel.EndTime < DateTime.Now && LoginUser.IsMember))
            {
                this.LoginUser = new Core.Model.LoginUser(userModel);
            }
            ViewBag.CarouseList = ICarouselService.GetList(x => !x.IsDelete).OrderByDescending(x => x.Sort).ToList();
            var tempelateList = ITemplateService.GetList(x => !x.IsDelete);
            var categoryIdList = tempelateList.Select(x => x.CategoryID).Distinct().ToList();
            var dic = ITemplateCategoryService.GetDic(x => categoryIdList.Contains(x.ID) && !x.IsDelete);
            tempelateList.ForEach(x =>
            {
                if (dic.ContainsKey(x.CategoryID))
                {
                    x.RotueName = dic[x.CategoryID].RouteName;
                    x.TemplateUrl = $"/Template/{dic[x.CategoryID].RouteName}/{x.ClassNo}";
                }
            });
            return View(tempelateList);
        }
        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <returns></returns>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        private bool CheckSignature()
        {
            string WeChat_Token = "47b2c135c8d856f31810a8cc3d53aca2";

            //从微信服务器接收传递过来的数据
            string signature = Request.QueryString["signature"]; //微信加密签名
            string timestamp = Request.QueryString["timestamp"];//时间戳
            string nonce = Request.QueryString["nonce"];//随机数
            string[] ArrTmp = { WeChat_Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);//将三个字符串组成一个字符串
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");//进行sha1加密
            tmpStr = tmpStr.ToLower();
            //加过密的字符串与微信发送的signature进行比较，一样则通过微信验证，否则失败。
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
        /// 验证微信API接口
        /// </summary>
        private void CheckWeChat()
        {
            string echoStr = Request.QueryString["echoStr"];

            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    Response.Write(echoStr);
                    Response.End();
                }
            }
        }
    }
}