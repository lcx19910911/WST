using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace WST.Core.MP
{
    public static class JsonExtend<T>
    {
        #region 私有方法
        /// <summary>
        /// 将实体List转换为Hashtable列表
        /// </summary>
        /// <param name="tlist"></param>
        /// <returns></returns>
        private static List<Hashtable> TlistToHash(IList<T> tlist)
        {
            var hashList = new List<Hashtable>();
            foreach (var item in tlist)
            {
                var hashItem = new Hashtable();
                PropertyInfo[] arrPropertyInfo = item.GetType().GetProperties();
                for (int i = 0; i < arrPropertyInfo.Length; i++)
                {
                    var name = arrPropertyInfo[i].Name;
                    var value = arrPropertyInfo[i].GetValue(item, null);
                    hashItem[name] = value;
                }
                hashList.Add(hashItem);
            }
            return hashList;
        }
        /// <summary>
        /// 将DataTable转换为Hashtable列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Hashtable> DataTableToHash(DataTable dt)
        {
            var hasList = new List<Hashtable>();
            foreach (DataRow item in dt.Rows)
            {
                var hashItem = new Hashtable();
                foreach (DataColumn colitem in dt.Columns)
                {
                    hashItem.Add(colitem.ColumnName,item[colitem]);
                }
                hasList.Add(hashItem);
            }
            return hasList;
        }
        #endregion 私有方法

        /// <summary>
        /// 对象转换成json格式
        /// </summary> 
        /// <returns></returns>
        public static string ToJson(object t)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return serializer.Serialize(t);
        }
        /// <summary>
        /// json格式转换成对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject(string json)
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<T>(json);
        }
        /// <summary>
        /// Datatable转化为json数据
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="flag">标记（"0"只显示List:'[{},{}]';"1"显示total,rows:{total:'',rows:[{},{},{}]}</param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt,int flag)
        {
            StringBuilder JsonString = new StringBuilder();     
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]");

                if(flag==0)
                    return JsonString.ToString();
                else
                    return "{\"total\":\"" + dt.Rows.Count + "\",\"rows\":" + JsonString.ToString() + "}";

            }
            else
            {
                if (flag == 0)
                    return "[]";
                else
                    return "{\"total\":\"0\",\"rows\":[]}";
            }

        }
        /// <summary>
        /// Datatable转化为json数据
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="flag">标记（"0"只显示List:'[{},{}]';"1"显示total,rows:{total:'',rows:[{},{},{}]}</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt, int flag,int recordCount)
        {
            StringBuilder JsonString = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]");

                if (flag == 0)
                    return JsonString.ToString();
                else
                    return "{\"total\":\"" + recordCount + "\",\"rows\":" + JsonString.ToString() + "}";

            }
            else
            {
                if (flag == 0)
                    return "[]";
                else
                    return "{\"total\":\"0\",\"rows\":[]}";
            }

        }        
        /// <summary>    
        /// List数据源转换为Json格式 
        /// </summary>
        /// <param name="source">数据源</param>    
        /// <param name="flag">标记（"0"只显示List:'[{},{}]';"1"显示total,rows:{total:'',rows:[{},{},{}]}）</param>
        /// <returns>字符窜</returns>   
        public static String ListToJson(List<T> source, int flag)
        {
            if (source == null || source.Count == 0)
            {
                if (flag == 0)
                    return "[]";
                if (flag == 1)
                    return "{\"total\":\"0\",\"rows\":[]}";
            }
            if (flag==0)
                return ToJson(source);
            if (flag==1)
                return "{\"total\":\"" + source.Count + "\",\"rows\":" + ToJson(source) + "}";
            return "[]";
        }
        /// <summary>    
        /// List数据源转换为Json格式 
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="flag">标记（"0"只显示List:'[{},{}]';"1"显示total,rows:{total:'',rows:[{},{},{}]}）</param>
        /// <param name="recordCount">记录数</param>
        /// <returns>字符窜</returns>   
        public static String ListToJson(List<T> source, int flag, int recordCount)
        {
            if (source == null || source.Count == 0)
            {
                if (flag == 0)
                    return "[]";
                if (flag == 1)
                    return "{\"total\":\"0\",\"rows\":\"\"}";
            }
            if (flag == 0)
                return ToJson(source);
            if (flag == 1)
                return "{\"total\":\"" + recordCount + "\",\"rows\":" + ToJson(source) + "}";
            return "[]";
        }
        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="list">线性数据</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static string ListToTreeJson(IList<T> tlist, string id, string pid)
        {
            List<Hashtable> hashList = TlistToHash(tlist);

            var h = new Hashtable(); //数据索引 
            var r = new List<Hashtable>(); //数据池,要返回的 
            foreach (var item in hashList)
            {
                if (!item.ContainsKey(id)) continue;
                h[item[id].ToString()] = item;
            }
            foreach (var item in hashList)
            {
                if (!item.ContainsKey(id)) continue;
                if (!item.ContainsKey(pid) || item[pid] == null || !h.ContainsKey(item[pid].ToString()))
                {
                    r.Add(item);
                }
                else
                {
                    var pitem = h[item[pid].ToString()] as Hashtable;
                    if (!pitem.ContainsKey("children"))
                        pitem["children"] = new List<Hashtable>();
                    var children = pitem["children"] as List<Hashtable>;
                    children.Add(item);
                }
            }
            return ToJson(r);
        }
        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="list">线性数据</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static List<Hashtable> ListToTree(IList<T> tlist, string id, string pid)
        {
            List<Hashtable> hashList = TlistToHash(tlist);

            var h = new Hashtable(); //数据索引 
            var r = new List<Hashtable>(); //数据池,要返回的 
            foreach (var item in hashList)
            {
                if (!item.ContainsKey(id)) continue;
                h[item[id].ToString()] = item;
            }
            foreach (var item in hashList)
            {
                if (!item.ContainsKey(id)) continue;
                if (!item.ContainsKey(pid) || item[pid] == null || !h.ContainsKey(item[pid].ToString()))
                {
                    r.Add(item);
                }
                else
                {
                    var pitem = h[item[pid].ToString()] as Hashtable;
                    if (!pitem.ContainsKey("children"))
                        pitem["children"] = new List<Hashtable>();
                    var children = pitem["children"] as List<Hashtable>;
                    children.Add(item);
                }
            }
            return r;
        }

        /// <summary>
        /// 枚举转为json数组
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="flag">是否追加全部</param>
        /// <returns></returns>
        public static string EnumToJson(Type enumType, bool flag)
        {
            if (!enumType.BaseType.Name.Equals("Enum"))
                return "[]";
            StringBuilder enumJson = new StringBuilder();
            enumJson.Append("[");
            if (flag)
                enumJson.Append("{\"text\":\"全部\",\"value\":\"\" },");
            foreach (int item in Enum.GetValues(enumType))
            {
                enumJson.Append("{\"text\":\"" + Enum.GetName(enumType, item) + "\",\"value\":\"" + item.ToString() + "\" },");
            }
            enumJson.Remove(enumJson.Length - 1, 1);
            enumJson.Append("]");
            return enumJson.ToString();
        }
    }
}
