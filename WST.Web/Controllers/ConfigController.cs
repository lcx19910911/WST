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
}