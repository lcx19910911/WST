﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WST.Core.Model;
using WST.Model;
using WST.Core;

namespace WST.IService
{
    /// <summary>
    /// 菜单
    /// </summary>
    public interface IMpAutoReplyService : IBaseService<MpAutoReply>
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        PageList<MpAutoReply> GetPageList(int pageIndex, int pageSize, string keyword);
        

    }
}
