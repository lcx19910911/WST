
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
    /// 秒杀
    /// </summary>
    public class MiaoShaService : BaseService<MiaoSha>, IMiaoShaService
    {
        public MiaoShaService()
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
        public PageList<MiaoSha> GetPageList(int pageIndex, int pageSize, string userId, string name, string userName)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.MiaoSha.Where(x => !x.IsDelete);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                if (userName.IsNotNullOrEmpty())
                {
                    var memberIDList = db.User.Where(x => !x.IsDelete && x.NickName.Contains(userName) && x.IsMember).Select(x => x.ID).ToList();
                    query = query.Where(x => memberIDList.Contains(x.UserID));
                }
                if (userId.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.UserID.Equals(userId));
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var userIdList = list.Select(x => x.UserID).Distinct().ToList();
                var userDic = db.User.Where(x => userIdList.Contains(x.ID)).ToDictionary(x => x.ID, x => (string.IsNullOrEmpty(x.StoreName) ? x.NickName : x.StoreName));
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
                return db.MiaoSha.Where(x => !x.IsDelete).OrderByDescending(x => x.CreatedTime).Select(x => new SelectItem()
                {
                    Text = x.Name,
                    Value = x.ID,
                }).ToList(); ;
            }
        }

        /// <summary>
        /// 秒杀
        /// </summary>
        /// <param name="storeIds"></param>
        /// <param name="inviteNo"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WebResult<bool> ToMiaoSha(string id, string name, string mobile)
        {
            using (var db = new DbRepository())
            {
                var model = db.MiaoSha.Find(id);
                if (model == null || model.IsDelete)
                {
                    return Result(false, Core.Code.ErrorCode.sys_param_format_error);
                }
                if ((model.StartTime < DateTime.Now && model.EndTime > DateTime.Now))
                {
                    if (model.UsedCount >= model.PrizeCount)
                    {
                        return Result(false, Core.Code.ErrorCode.prize_not_had);
                    }
                    model.UsedCount++;
                    db.UserActivity.Add(new UserActivity()
                    {
                        Amount = model.LessPrice,
                        PrizeInfo = $"{Client.LoginUser.Account}在{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}参加秒杀{model.Name},成功秒杀",
                        Code = TargetCode.Miaosha,
                        JoinUserID = Client.LoginUser.ID,
                        TargetID = model.ID,
                        IsPrize = true,
                        JoinUserName = name,
                        Openid = Client.LoginUser.Openid,
                        Mobile = name,
                        ShopUserID = model.UserID
                    });
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        return Result(true);
                    }
                    else
                    {
                        return Result(false, ErrorCode.sys_fail);
                    }
                }
                else
                {
                    return Result(false, Core.Code.ErrorCode.activity_time_out);
                }
            }
        }
    }
}
