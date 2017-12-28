
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
    public class MpMenuService : BaseService<MpMenu>, IMpMenuService
    {
        public MpMenuService()
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
        public PageList<MpMenu> GetPageList(int pageIndex, int pageSize)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.MpMenu.Where(x => !x.IsDelete);
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var parentIdList = list.Select(x => x.ParentID).ToList();
                var menuDic = db.MpMenu.Where(x => parentIdList.Contains(x.ID)).ToDictionary(x => x.ID, x => x.Name);
                list.ForEach(x =>
                {
                    if (x.ParentID.IsNotNullOrEmpty() && menuDic.ContainsKey(x.ParentID))
                    {
                        x.ParentName = menuDic[x.ParentID];
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }

        public new WebResult<bool> Add(MpMenu model)
        {
            var msg = CheckIsNullOrEmpty(model);
            if (msg.IsNotNullOrEmpty())
            {
                return Result(false, msg);
            }
            using (var db = new DbRepository())
            {
                if (model.ParentID.IsNullOrEmpty())
                {
                    if (db.MpMenu.Any(x => string.IsNullOrEmpty(x.ParentID) && x.Name == model.Name))
                    {
                        return Result(false, "自定义菜单名称已存在");
                    }

                    if (db.Menu.Count(x => string.IsNullOrEmpty(x.ParentID)) >= 3)
                    {
                        return Result(false, "自定义菜单一级菜单只能有三个");
                    }
                }
                else
                {
                    if (db.MpMenu.Any(x => x.ParentID == model.ParentID && x.Name == model.Name))
                    {
                        return Result(false, "自定义菜单名称已存在");
                    }

                    if (db.Menu.Any(x => x.ID == x.ParentID))
                    {
                        return Result(false, "自定义菜单父级菜单不存在");
                    }

                    if (db.Menu.Count(x => x.ParentID == model.ParentID) >= 5)
                    {
                        return Result(false, "自定义菜单二级菜单只能有五个");
                    }
                }
                //给key默认的前缀
                if (model.EventType == Enum_Button_Type.点击事件)
                {
                    //if (model.KeyType == Enum_WXServiceMsg_Type.视频消息)
                    //    model.EventKey = "video_" + model.EventKey;
                     if (model.KeyType == Enum_WXServiceMsg_Type.图文消息)
                        model.EventKey = "text&pic_" + model.EventKey;
                    //else if (model.KeyType == Enum_WXServiceMsg_Type.图片消息)
                    //    model.EventKey = "pic_" + model.EventKey;
                    //else if (model.KeyType == Enum_WXServiceMsg_Type.音乐消息)
                    //    model.EventKey = "music_" + model.EventKey;
                    //else if (model.KeyType == Enum_WXServiceMsg_Type.语音消息)
                        //model.EventKey = "sound_" + model.EventKey;
                    //用户自定义key时候  不能有默认的 key存在
                    else if (model.EventKey == "video" || model.EventKey == "text&pic" || model.EventKey == "pic" || model.EventKey == "music" || model.EventKey == "sound")
                    {
                        return Result(false, "自定义菜单自定义EventKey不能为" + model.EventKey);
                    }
                }

                db.MpMenu.Add(model);
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

        public WebResult<bool> Update(MpMenu model)
        {
            var msg = CheckIsNullOrEmpty(model);
            if (msg.IsNotNullOrEmpty())
            {
                return Result(false, msg);
            }
            using (var db = new DbRepository())
            {
                var oldModel = db.MpMenu.Find(model.ID);

                if (model.ParentID.IsNullOrEmpty())
                {
                    if (db.MpMenu.Any(x => string.IsNullOrEmpty(x.ParentID) && x.Name == model.Name))
                    {
                        return Result(false, "自定义菜单名称已存在");
                    }

                    if (db.Menu.Count(x => string.IsNullOrEmpty(x.ParentID)) >= 3)
                    {
                        return Result(false, "自定义菜单一级菜单只能有三个");
                    }
                }
                else
                {
                    if (db.MpMenu.Any(x => x.ParentID == model.ParentID && x.Name == model.Name))
                    {
                        return Result(false, "自定义菜单名称已存在");
                    }

                    if (db.Menu.Any(x => x.ID == x.ParentID))
                    {
                        return Result(false, "自定义菜单父级菜单不存在");
                    }

                    if (db.Menu.Count(x => x.ParentID == model.ParentID) >= 5)
                    {
                        return Result(false, "自定义菜单二级菜单只能有五个");
                    }
                }
                //给key默认的前缀
                if (model.EventType == Enum_Button_Type.点击事件)
                {
                    //if (model.KeyType == Enum_WXServiceMsg_Type.视频消息)
                    //    model.EventKey = "video_" + model.EventKey;
                     if (model.KeyType == Enum_WXServiceMsg_Type.图文消息)
                        model.EventKey = "text&pic_" + model.EventKey;
                    //else if (model.KeyType == Enum_WXServiceMsg_Type.图片消息)
                    //    model.EventKey = "pic_" + model.EventKey;
                    //else if (model.KeyType == Enum_WXServiceMsg_Type.音乐消息)
                    //    model.EventKey = "music_" + model.EventKey;
                    //else if (model.KeyType == Enum_WXServiceMsg_Type.语音消息)
                    //    model.EventKey = "sound_" + model.EventKey;
                    //用户自定义key时候  不能有默认的 key存在
                    else if (model.EventKey == "video" || model.EventKey == "text&pic" || model.EventKey == "pic" || model.EventKey == "music" || model.EventKey == "sound")
                    {
                        return Result(false, "自定义菜单自定义EventKey不能为" + model.EventKey);
                    }
                }

                oldModel.Name = model.Name;
                oldModel.ParentID = model.ParentID;
                oldModel.ReplyText = model.ReplyText;
                oldModel.KeyType = model.KeyType;
                oldModel.EventType = model.EventType;
                oldModel.EventKey = model.EventKey;
                oldModel.LinkUrl = model.LinkUrl;
                oldModel.Sort = model.Sort;
                if (db.SaveChanges() > 0)
                {
                    return Result(true);
                }
                else
                {
                    return Result(false);
                }
            }

        }



        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public List<SelectItem> GetSelectList(string id)
        {
            using (DbRepository db = new DbRepository())
            {
                return db.MpMenu.Where(x => !x.IsDelete&&x.ParentID==id).OrderByDescending(x => x.Sort).Select(x => new SelectItem()
                {
                    Text = x.Name,
                    Value = x.ID,
                }).ToList(); ;
            }
        }

        /// <summary>
        /// 判断数据非空
        /// </summary>
        /// <returns></returns>
        public string CheckIsNullOrEmpty(MpMenu model)
        {
            if (model == null)
                return "自定义菜单数据为空";
            if (model.Name.Length > 4 && model.ParentID.IsNullOrEmpty())
                return "自定义菜单一级菜单名称超过4个字符";

            if (model.Name.Length > 20 && model.ParentID.IsNotNullOrEmpty())
                return "自定义菜单二级菜单名称超过20个字符";

            if (!string.IsNullOrEmpty(model.EventKey) && model.EventKey.Length > 50)
                return "自定义菜单事件Key值超过50个字符";

            if (model.EventType == Enum_Button_Type.点击事件)
            {
                if (string.IsNullOrEmpty(model.EventKey))
                    return "自定义菜单点击事件没有对应的Key值";
                if (model.KeyType == Enum_WXServiceMsg_Type.文本消息 && string.IsNullOrEmpty(model.ReplyText))
                    return "自定义菜单点击事件回复文本为空";
                if (model.KeyType == Enum_WXServiceMsg_Type.文本消息 && model.ReplyText.Length > 200)
                    return "自定义菜单点击事件回复文本超过200字符";
            }
            else if (model.EventType == Enum_Button_Type.跳转页面)
            {
                if (string.IsNullOrEmpty(model.LinkUrl))
                    return "自定义菜单页面跳转事件没有跳转的链接";
            }

            if (!string.IsNullOrEmpty(model.LinkUrl) && model.LinkUrl.Length > 100)
                return "自定义菜单跳转URL超过100个字符";

            if (model.ParentID.IsNotNullOrEmpty() && model.EventType != Enum_Button_Type.点击事件 && model.EventType != Enum_Button_Type.跳转页面)
                return "自定义菜单类型不是定义类型";

            return "";
        }


        #region 提交菜单到微信
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="adminId">管理员id</param>
        /// <param name="appId">公众号appid</param>
        /// <returns></returns>
        public WebResult<bool> CreateMenu(string token)
        {
            //提交的微信格式类集合
            List<MenuButton> menuList = new List<MenuButton>();
            var list = GetList(x => !x.IsDelete);
            //加载第一级菜单
            List<MpMenu> firstLevelMenu = list.Where(x => string.IsNullOrEmpty(x.ParentID)).ToList();

            if (firstLevelMenu != null && firstLevelMenu.Count > 0)
            {
                foreach (var item in firstLevelMenu)
                {
                    MenuButton menu = new MenuButton();
                    //查找该菜单是否有子级菜单
                    List<MpMenu> childrenList = list.Where(x => x.ParentID == item.ID).ToList();
                    if (childrenList != null && childrenList.Count != 0)
                    {
                        //有菜单 加载二级菜单 
                        menu = MpMenuConvertToWxMenu(item, true);
                        menu.SubButtonList = ListMpMenuConvertToWxMenu(childrenList);
                    }
                    else
                    {
                        menu = MpMenuConvertToWxMenu(item, false);
                    }
                    menuList.Add(menu);
                }
            }


            //发送
            MenuFunction fuc = new MenuFunction();
            if (fuc.MenuDelete(token)["state"].ToString() == "success")
            {
                if (fuc.MenuCreate(token, menuList)["errcode"].ToString() != "0")
                    return Result(true);
                else
                    return Result(false);
            }
            else
                return Result(false, "提交菜单到微信服务器出错");
        }
        #endregion

        #region MpMenu类转为微信菜单格式对象

        /// <summary>
        /// MpMenu类转为微信菜单格式对象 第一级菜单只加载菜单名
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isFirstMenu">是否是第一级菜单 </param>
        /// <returns></returns>
        public MenuButton MpMenuConvertToWxMenu(MpMenu model, bool isFirstMenu)
        {
            MenuButton menu = new MenuButton()
            {
                Name = model.Name
            };
            if (!isFirstMenu)
            {
                if (model.EventType == Enum_Button_Type.点击事件)
                {
                    menu.ButtonType = "click";
                    menu.Key = model.EventKey;
                }
                else if (model.EventType == Enum_Button_Type.跳转页面)
                {
                    menu.Url = model.LinkUrl;
                    menu.ButtonType = "view";
                }
            }
            return menu;
        }

        /// <summary>
        /// MpMenu类转为微信菜单格式对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MenuButton> ListMpMenuConvertToWxMenu(List<MpMenu> list)
        {
            List<MenuButton> returnList = new List<MenuButton>();

            foreach (var model in list)
            {
                MenuButton menu = new MenuButton()
                {
                    Name = model.Name
                };
                if (model.EventType == Enum_Button_Type.点击事件)
                {
                    menu.ButtonType = "click";
                    menu.Key = model.EventKey;
                }
                else if (model.EventType == Enum_Button_Type.跳转页面)
                {
                    menu.Url = model.LinkUrl;
                    menu.ButtonType = "view";
                }
                returnList.Add(menu);
            }
            return returnList;
        }

        #endregion
    }
}
