
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WST.Core.Model;
using WST.Model;

namespace WST.IService
{
    public interface IRechargePlanService : IBaseService<RechargePlan>
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        PageList<RechargePlan> GetPageList(int pageIndex, int pageSize, string name);

        List<RechargePlan> GetList();

    }
}
