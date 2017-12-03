using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WST.Core.MP.Menu
{
    /// <summary>
    /// 微信自定义菜单按钮
    /// </summary>
    public class MenuButton
    {
        public MenuButton()
        {
            this.SubButtonList = new List<MenuButton>();
        }

        /// <summary>
        /// 菜单的响应动作类型
        /// </summary>
        public string ButtonType { get; set; }
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 网页链接，用户点击菜单可打开链接，不超过256字节
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 多媒体ID
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 子级菜单
        /// </summary>
        public List<MenuButton> SubButtonList { get; set; }
    }
}