
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
    /// 课程分类
    /// </summary>
    public class UserActivityService : BaseService<UserActivity>, IUserActivityService
    {
        public UserActivityService()
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
        public PageList<UserActivity> GetPageList(int pageIndex, int pageSize,string targetId,string userId,string joinUserId,string joinUserName, TargetCode? code)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.UserActivity.Where(x => !x.IsDelete);
                if (targetId.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.TargetID.Equals(targetId));
                }
                if (userId.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.ShopUserID.Equals(userId));
                }
                if (joinUserId.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.JoinUserID.Equals(joinUserId));
                }
                if (joinUserName.IsNotNullOrEmpty())
                {
                    var memberIDList = db.User.Where(x => !x.IsDelete && x.NickName.Contains(joinUserName) && x.IsMember).Select(x => x.ID).Distinct().ToList();
                    query = query.Where(x => memberIDList.Contains(x.JoinUserID));
                }
                if (code!=null&&(int)code!=-1)
                {
                    query = query.Where(x => x.Code== code);
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                list.ForEach(x =>
                {
                });

                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public List<UserActivity> GetSelectList(TargetCode code,string targetId)
        {
            using (DbRepository db = new DbRepository())
            {
                return db.UserActivity.Where(x => !x.IsDelete&& code==x.Code&&x.TargetID==targetId).OrderByDescending(x=>x.CreatedTime).ToList(); ;
            }
        }
    }
}
