
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
    /// 充值方案
    /// </summary>
    public class RechargePlanService : BaseService<RechargePlan>, IRechargePlanService
    {
        public RechargePlanService()
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
        public PageList<RechargePlan> GetPageList(int pageIndex, int pageSize, string name)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.RechargePlan.Where(x => !x.IsDelete);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public List<RechargePlan> GetList()
        {
            using (DbRepository db = new DbRepository())
            {
                return db.RechargePlan.Where(x => !x.IsDelete).ToList(); ;
            }
        }
    }
}
