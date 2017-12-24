
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WST.Core;
using WST.Core.Code;
using WST.Core.Extensions;
using WST.Core.Helper;
using WST.Core.Model;
using WST.Core.Web;
using WST.DB;
using WST.IService;
using WST.Model;

namespace WST.Service
{
    /// <summary>
    /// 拼团
    /// </summary>
    public class PinTuanService : BaseService<PinTuan>, IPinTuanService
    {
        public PinTuanService()
        {
            base.ContextCurrent = HttpContext.Current;
        }
        
       

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        public PageList<PinTuan> GetPageList(int pageIndex, int pageSize,string userId, string name,string userName)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.PinTuan.Where(x => !x.IsDelete);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                if (userName.IsNotNullOrEmpty())
                {
                    var memberIDList = db.User.Where(x => !x.IsDelete && x.NickName.Contains(userName)&&x.IsMember).Select(x => x.ID).Distinct().ToList();
                    query = query.Where(x => memberIDList.Contains(x.UserID));
                }
                if (userId.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.UserID.Equals(userId));
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var userIdList = list.Select(x => x.UserID).Distinct().ToList();
                var userDic = db.User.Where(x => userIdList.Contains(x.ID)).ToDictionary(x => x.ID, x => x.NickName);
                list.ForEach(x =>
                {
                    if (userDic.ContainsKey(x.UserID))
                    {
                        x.UserName = userDic[x.UserID];
                    }
                });


                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public List<SelectItem> GetSelectList()
        {
            using (DbRepository db = new DbRepository())
            {
                return db.PinTuan.Where(x => !x.IsDelete).OrderByDescending(x=>x.CreatedTime).Select(x => new SelectItem()
                {
                    Text = x.Name,
                    Value = x.ID,
                }).ToList(); ;
            }
        }
    }
}
