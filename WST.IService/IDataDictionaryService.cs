﻿
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
    public interface IDataDictionaryService : IBaseService<DataDictionary>
    {
        PageList<DataDictionary> GetPageList(int pageIndex, int pageSize, string parentKey, GroupCode code, string name, string value);
        List<SelectItem> GetSelectList(GroupCode code, string value);

        Dictionary<GroupCode, Dictionary<string, DataDictionary>> CacheDic();
    }
}
