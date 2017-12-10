
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
    /// 微信用户
    /// </summary>
    public class UserService : BaseService<User>, IUserService
    {
        public UserService()
        {
            base.ContextCurrent = HttpContext.Current;
        }
        

        /// <summary>
        /// 编辑管理用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Manager(User model)
        {
            var user = Find(model.ID);
            if (user == null)
            {
                model.CreatedTime = DateTime.Now; 
                int id=Add(model);
                if (id > 0)
                {
                    LoginHelper.CreateUser(new LoginUser(model), Params.UserCookieName);
                    return Result(true);
                }
                else
                    return Result(false, ErrorCode.sys_fail);
            }
            else
            {
                
                user.NickName = model.NickName;
                int result=Update(user);
                if (result > 0)
                {
                    return Result(true);
                }
                else
                    return Result(false, ErrorCode.sys_fail);
            }

        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public User FindByOpenId(string openId)
        {
            if (string.IsNullOrEmpty(openId))
                return null;
            return Find(x => x.OpenID == openId);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        public PageList<User> GetPageList(int pageIndex, int pageSize, string name,string phone, bool isMember, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.User.Where(x => !x.IsDelete&&x.IsMember==isMember);
                var admin = db.Admin.Find(Client.LoginAdmin.ID);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.NickName.Contains(name));
                }
                if (phone.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Mobile.Contains(phone));
                }
                if (createdTimeStart != null)
                {
                    query = query.Where(x => x.CreatedTime >= createdTimeStart);
                }
                if (createdTimeEnd != null)
                {
                    createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                    query = query.Where(x => x.CreatedTime < createdTimeEnd);
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                //门店
                var userIdList = list.Select(x => x.ID).ToList();
              

                //顾问
                var adviserIdList = list.Select(x => x.AdviserID).ToList();
                var adviserDic = db.Adviser.Where(x => !x.IsDelete && adviserIdList.Contains(x.ID)).ToDictionary(x => x.ID, x => x.Name);
                list.ForEach(x =>
                {
                    if (x.AdviserID.IsNotNullOrEmpty() && adviserDic.ContainsKey(x.AdviserID))
                    {
                        x.AdviserName = adviserDic[x.AdviserID];
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }
    }
}
