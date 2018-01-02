
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
        public PageList<UserActivity> GetPageList(int pageIndex, int pageSize,string targetId,string userId,string joinUserId,string joinUserName,bool? isPrize, TargetCode? code, bool? isUser = null, bool? isOther = null, string targetUserId = "")
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
                if (isPrize != null)
                {
                    query = query.Where(x => x.IsPrize == isPrize);
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

                if (isUser != null&&isUser.Value)
                {
                    if (isOther != null)
                    {
                        if (isOther.Value)
                        {
                            query = query.Where(x => x.TargetUserID == targetUserId);
                        }
                        else
                        {
                            query = query.Where(x => string.IsNullOrEmpty(x.TargetUserID));
                        }
                    }
                    else
                    {
                        query = query.Where(x => string.IsNullOrEmpty(x.TargetUserID));
                    }
                }
                else
                {
                    if (targetUserId.IsNotNullOrEmpty())
                    {
                        query = query.Where(x =>x.TargetUserID==targetUserId);
                    }
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
               ;
                if (list != null && list.Count > 0)
                {
                    var actCode = list.FirstOrDefault().Code;
                    if (actCode == TargetCode.Pintuan)
                    {
                        var model = db.PinTuan.Find(targetId);
                        if (model == null)
                        {
                            return null;;
                        }
                        var priceList = model.PinTuanItemJson.DeserializeJson<List<PinTuanItem>>().OrderBy(x => x.Count).ToList();
                        var countList = priceList.Select(x => x.Count).ToList();
                        var price = 0M;
                        for (var index = 1; index <= countList.Count; index++)
                        {
                            if (index < countList.Count)
                            {
                                if (model.JoinCount < countList[index - 1])
                                {
                                    price = model.OldPrice;
                                    break;
                                }
                                if (model.JoinCount == countList[index - 1])
                                {
                                    price = priceList[index - 1].Amount;
                                    break;
                                }
                                if (model.JoinCount > countList[index - 1] && model.JoinCount < countList[index])
                                {
                                    price = priceList[index - 1].Amount;
                                    break;
                                }
                            }
                            else
                            {
                                price = priceList[index - 1].Amount;
                            }
                        }
                        list.ForEach(x =>
                        {
                            x.Amount = price;
                        });
                    }
                }
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
