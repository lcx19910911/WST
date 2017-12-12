
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
    /// 轮播图
    /// </summary>
    public class CarouselService : BaseService<Carousel>, ICarouselService
    {
        public CarouselService()
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
        public PageList<Carousel> GetPageList(int pageIndex, int pageSize, string title)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.Carousel.Where(x => !x.IsDelete);
                if (title.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Title.Contains(title));
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.Sort).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
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
        public List<SelectItem> GetSelectList()
        {
            using (DbRepository db = new DbRepository())
            {
                return db.Carousel.Where(x => !x.IsDelete).OrderByDescending(x=>x.Sort).Select(x => new SelectItem()
                {
                    Text = x.Title,
                    Value = x.Url,
                }).ToList(); ;
            }
        }
    }
}
