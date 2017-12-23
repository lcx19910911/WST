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
using WST.Web.Framework.Filters;
using WxPayAPI;
using WST.Core.Util;
using WST.Model;
using WST.Core.Helper;

namespace WST.Web.Controllers
{
    /// <summary>
    /// 默认配置
    /// </summary>
    public class ConfigController : BaseUserController
    {
        public IUserService IUserService;
        public IDataDictionaryService IDataDictionaryService;
        public IRechargePlanService IRechargePlanService;
        public IMusicService IMusicService;

        public ConfigController(IUserService _IUserService, IRechargePlanService _IRechargePlanService, IMusicService _IMusicService, IDataDictionaryService _IDataDictionaryService)
        {
            this.IUserService = _IUserService;
            this.IRechargePlanService = _IRechargePlanService;
            this.IMusicService = _IMusicService;
            this.IDataDictionaryService = _IDataDictionaryService;
        }
    
        public ActionResult GetFuncList()
        {
            return JResult(IDataDictionaryService.GetSelectList(GroupCode.Function,""));
        }

        public ActionResult SendMsg(string mobile)
        {
            //var sendList = CacheHelper.Get<List<string>>("sendList");
            //if (sendList == null || sendList.Count == 0)
            //{
            //    CacheHelper.Set<List<string>>("sendList", new List<string>() { mobile }, CacheTimeOption.OneMinute);
            //}
            //else
            //{
            //    if (sendList.Contains(mobile))
            //    {
            //        return JResult(0);
            //    }
            //    else
            //    {
            //        sendList.Add(mobile);
            //    }
            //}
            CacheHelper.Remove("mobile_" + mobile);
            var code=CacheHelper.Get<int>("mobile_" + mobile, CacheTimeOption.SixMinutes, () =>
            {
                return new Random().Next(10000, 999999);
            });
            var result=WebHelper.GetPage($"http://v.juhe.cn/sms/send?mobile={mobile}&tpl_id=52909&tpl_value={HttpUtility.UrlEncode("#code#=" + code)}&key=701db295de1c161b619cb3b85fe5a65d");
            return JResult(result.DeserializeJson<JuheResult>()?.error_code==0);
        }


        

        public ActionResult GetMusicList()
        {
            var categoryList = IDataDictionaryService.GetSelectList(GroupCode.MusicCategory, "");
            var musicDic = IMusicService.GetList().GroupBy(x=>x.CategoryID).ToDictionary(x=>x.Key,x=>x.ToList());
            categoryList.ForEach(x =>
            {
                if (musicDic.ContainsKey(x.Value))
                {
                    x.Data = musicDic[x.Value];
                }
            });
            return JResult(categoryList);
        }
    }

    public class JuheResult
    {
        public int error_code { get; set; }
        public string reason { get; set; }
    }
}