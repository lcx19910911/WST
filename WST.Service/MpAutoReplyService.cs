
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
using WST.Core.MP.ReplyMsg.Message;
using WST.Core.MP.ReplyMsg;
using WST.Core.MP.Menu;

namespace WST.Service
{
    /// <summary>
    /// 微信菜单
    /// </summary>
    public class MpAutoReplyService : BaseService<MpAutoReply>, IMpAutoReplyService
    {
        public MpAutoReplyService()
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
        public PageList<MpAutoReply> GetPageList(int pageIndex, int pageSize,string keyword)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.MpAutoReply.Where(x => !x.IsDelete);
                if (keyword.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Keyword.Contains(keyword));
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                //var parentIdList = list.Select(x => x.ParentID).ToList();
                //var menuDic = db.MpAutoReply.Where(x => parentIdList.Contains(x.ID)).ToDictionary(x => x.ID, x => x.Name);
                list.ForEach(x =>
                {
                    
                });

                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }

        public new WebResult<bool> Add(MpAutoReply model)
        {
            var msg = CheckIsNullOrEmpty(model);
            if (msg.IsNotNullOrEmpty())
            {
                return Result(false, msg);
            }
            using (var db = new DbRepository())
            {
                
                db.MpAutoReply.Add(model);
                if (db.SaveChanges() > 0)
                {
                    return Result(true);
                }
                else
                {
                    return Result(false, ErrorCode.sys_fail);
                }
            }

        }

        public WebResult<bool> Update(MpAutoReply model)
        {
            var msg = CheckIsNullOrEmpty(model);
            if (msg.IsNotNullOrEmpty())
            {
                return Result(false, msg);
            }
            using (var db = new DbRepository())
            {
                var oldModel = db.MpAutoReply.Find(model.ID);

                oldModel.AutoReplyType = model.AutoReplyType;
                oldModel.MaterialType = model.MaterialType;
                oldModel.Keyword = model.Keyword;
                oldModel.PerfectMatch = model.PerfectMatch;
                oldModel.Details = model.Details;
                oldModel.FilePath = model.FilePath;
                oldModel.MediaId = model.MediaId;
                if (db.SaveChanges() > 0)
                {
                    return Result(true);
                }
                else
                {
                    return Result(false, ErrorCode.sys_fail);
                }
            }

        }

        /// <summary>
        /// 判断数据非空
        /// </summary>
        /// <returns></returns>
        public string CheckIsNullOrEmpty(MpAutoReply model)
        {
            if (model == null)
                return "图文素材数据为空";

            if (!string.IsNullOrEmpty(model.Keyword) && model.Keyword.Length > 50)
                return "自动回复关键字超过50个字符" ;

            if (model.AutoReplyType != Enum_AutoReplay_Type.关键字 && model.AutoReplyType != Enum_AutoReplay_Type.关注 && model.AutoReplyType != Enum_AutoReplay_Type.默认)
            {
                return "自动回复类型不是定义的类型" ;
            }
            else
            {
                if (model.AutoReplyType == Enum_AutoReplay_Type.关注 || model.AutoReplyType == Enum_AutoReplay_Type.默认)
                {
                    if (string.IsNullOrEmpty(model.Details))
                        return "自动回复内容为空";
                    else if (model.Details.Length > 500)
                        return "自动回复内容超过500个字符";
                }
                else if (model.AutoReplyType == Enum_AutoReplay_Type.关键字)
                {
                    if (string.IsNullOrEmpty(model.Keyword))
                        return "自动回复关键字为空";
                    else if (model.Keyword.Length > 50)
                        return "自动回复关键字超过50个字符";

                    if (model.MaterialType == Enum_Material_Type.图片)
                    {
                        if (string.IsNullOrEmpty(model.MediaId))
                            return "自动回复图片MediaId为空";
                        else if (model.MediaId.Length > 50)
                            return "自动回复图片MediaId超过50个字符";
                    }
                }
            }

            return "";
        }

        
    }
}
