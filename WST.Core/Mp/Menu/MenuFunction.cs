using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WST.Core.Helper;
using WST.Core.Extensions;
using WST.Core.Code;

namespace WST.Core.MP.Menu
{
    /// <summary>
    /// 自定义菜单方法
    /// </summary>
    public class MenuFunction
    {
        #region 创建自定义菜单
        /// <summary>
        /// 创建自定义菜单
        /// 返回Hashtable对象：{"errcode":错误代码,"errmsg":错误消息}
        /// state:success,error分别代表成功，失败
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="btnList"></param>
        /// <returns></returns>
        public Hashtable MenuCreate(string accessToken, List<MenuButton> btnList)
        {
            string wxurl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + accessToken;

            List<object> leverOneBtnList = new List<object>();  //  一级按钮

            #region 构造按钮结构
            foreach (var item in btnList)
            {
                if (item.SubButtonList == null || item.SubButtonList.Count == 0)
                {
                    if (item.ButtonType == "view")
                    {
                        var btn = new { type = item.ButtonType.ToString(), name = item.Name, url = item.Url };
                        leverOneBtnList.Add(btn);
                    }
                    else if (item.ButtonType == "media_id")
                    {
                        var btn = new { type = item.ButtonType.ToString(), name = item.Name, media_id = item.MediaId };
                        leverOneBtnList.Add(btn);
                    }
                    else
                    {
                        var btn = new { type = item.ButtonType.ToString(), name = item.Name, key = item.Key };
                        leverOneBtnList.Add(btn);
                    }
                }
                else
                {
                    List<object> leverTwoBtnList = new List<object>();  // 二级按钮                    
                    foreach (var sitem in item.SubButtonList)
                    {
                        if (sitem.ButtonType == "view")
                        {
                            var btn = new { type = sitem.ButtonType.ToString(), name = sitem.Name, url = sitem.Url };
                            leverTwoBtnList.Add(btn);
                        }
                        else if (item.ButtonType == "media_id")
                        {
                            var btn = new { type = item.ButtonType.ToString(), name = item.Name, media_id = item.MediaId };
                            leverTwoBtnList.Add(btn);
                        }
                        else
                        {
                            var btn = new { type = sitem.ButtonType.ToString(), name = sitem.Name, key = sitem.Key };
                            leverTwoBtnList.Add(btn);
                        }
                    }

                    leverOneBtnList.Add(new { name = item.Name, sub_button = leverTwoBtnList });
                }
            }
            #endregion

            Hashtable btnHash = new Hashtable();
            btnHash.Add("button", leverOneBtnList);

            string result = Web.WebHelper.GetPage(wxurl, btnHash.ToJson(), "POST");
            Hashtable resHash = result.DeserializeJson<Hashtable>();
            resHash["errmsg"]=MpErrorCode.GetErrmsg(resHash["errcode"].ToString());
            return resHash;
        }
        #endregion

        #region 自定义菜单查询
        /// <summary>
        /// 自定义菜单查询
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public List<MenuButton> GetMenu(string accessToken)
        {
            string wxurl = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + accessToken;
            string result = Web.WebHelper.GetPage(wxurl, "", "GET");

            object[] firstBtnArr = (JsonExtend<Hashtable>.ToObject(result)["menu"] as Dictionary<string, object>)["button"] as object[];
            List<MenuButton> list = new List<MenuButton>();

            foreach (Dictionary<string, object> firstItem in firstBtnArr)
            {
                MenuButton firstBtn = new MenuButton();
                firstBtn.Name = firstItem["name"].ToString();

                if (firstItem.ContainsKey("sub_button"))
                {
                    object[] secondBtnArr = firstItem["sub_button"] as object[];
                    List<MenuButton> secondBtnList = new List<MenuButton>();

                    foreach (Dictionary<string, object> secondItem in secondBtnArr)
                    {
                        MenuButton secondBtn = new MenuButton();

                        secondBtn.Name = secondItem["name"].ToString();
                        if (secondItem.ContainsKey("sub_button"))
                        {
                            object[] thirdBtnArr = secondItem["sub_button"] as object[];
                            List<MenuButton> thirdBtnList = new List<MenuButton>();
                            foreach (Dictionary<string, object> thirdItem in thirdBtnArr)
                            {
                                MenuButton thirdBtn = new MenuButton();
                                thirdBtn.Name = thirdItem["name"].ToString();
                                thirdBtn.ButtonType = thirdItem["type"].ToString();
                                if (secondBtn.ButtonType == "view")
                                    secondBtn.Url = thirdItem["url"].ToString();
                                else
                                    secondBtn.Key = thirdItem["key"].ToString();
                            }
                            secondBtn.SubButtonList = thirdBtnList;
                        }
                        else
                        {
                            secondBtn.ButtonType = secondItem["type"].ToString();
                            if (secondBtn.ButtonType == "view")
                                secondBtn.Url = secondItem["url"].ToString();
                            else
                                secondBtn.Key = secondItem["key"].ToString();
                        }
                        secondBtnList.Add(secondBtn);
                    }
                    firstBtn.SubButtonList = secondBtnList;
                }
                else
                {
                    firstBtn.ButtonType = firstItem["type"].ToString();
                    if (firstBtn.ButtonType == "view")
                        firstBtn.Url = firstItem["url"].ToString();
                    else
                        firstBtn.Key = firstItem["key"].ToString();
                }

                list.Add(firstBtn);
            }

            return list;
        }
        #endregion

        #region 删除自定义菜单
        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public Hashtable MenuDelete(string accessToken)
        {
            Hashtable retHash = new Hashtable();

            string wxurl = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + accessToken;
            string result = Web.WebHelper.GetPage(wxurl, "", "GET");
            Hashtable resHash = JsonExtend<Hashtable>.ToObject(result);

            if (resHash["errcode"].GetInt() == 0)
            {
                retHash.Add("state", "success");
            }
            else
            {
                retHash.Add("state", "error");
                retHash.Add("errcode", resHash["errcode"]);
                retHash.Add("errmsg", resHash["errmsg"]);
            }
            return retHash;
        }
        #endregion
    }
}