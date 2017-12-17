
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
    /// 模板
    /// </summary>
    public class TemplateService : BaseService<Template>, ITemplateService
    {
        public TemplateService()
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
        public PageList<Template> GetPageList(int pageIndex, int pageSize, string name, string categoryName)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.Template.Where(x => !x.IsDelete);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                if (categoryName.IsNotNullOrEmpty())
                {
                    var categoryIDList = db.TemplateCategory.Where(x => !x.IsDelete && x.Name.Contains(categoryName)).Select(x => x.ID).Distinct().ToList();
                    query = query.Where(x => categoryIDList.Contains(x.CategoryID));
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var categoryIdsList = list.Select(x => x.CategoryID).Distinct().ToList();
                var categoryDic = db.TemplateCategory.Where(x => categoryIdsList.Contains(x.ID)).ToDictionary(x => x.ID);
                list.ForEach(x =>
                {
                    if (x.CategoryID.IsNotNullOrEmpty() && categoryDic.ContainsKey(x.CategoryID))
                    {
                        x.CategoryName = categoryDic[x.CategoryID].Name;
                        x.TemplateUrl = $"/Template/{categoryDic[x.CategoryID].RouteName}/{x.ClassNo}";
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }

        public List<Template> GetList(string categoryId)
        {
            using (DbRepository db = new DbRepository())
            {
                return db.Template.Where(x => !x.IsDelete && x.CategoryID == categoryId).OrderBy(x => x.CreatedTime).ToList(); ;
            }
        }
        public Dictionary<string, List<Template>> GetDic(List<string> categoryIds)
        {
            using (DbRepository db = new DbRepository())
            {
                var model = new Dictionary<string, List<Template>>();
                var dic= db.Template.Where(x => !x.IsDelete && categoryIds.Contains(x.CategoryID)).GroupBy(x=>x.CategoryID).ToDictionary(x=>x.Key,x=>x.ToList()) ;
                categoryIds.ForEach(x =>
                {
                    if (dic.ContainsKey(x))
                    {
                        model.Add(x, dic[x]);
                    }
                });
                return model;
            }
        }
    }
}
