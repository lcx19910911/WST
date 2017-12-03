using WST.Core.MP.ReplyMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WST.Core.MP.ReplyMsg.Message
{
    /// <summary>
    /// 图文消息
    /// </summary>
    public class ReplyNewsMsg : ReplyMessage
    {
        /// <summary>
        /// 图文消息个数，限制为10条以内
        /// </summary>
        public int ArticleCount { get; set; }
        /// <summary>
        /// 多条图文消息信息，默认第一个item为大图,注意，如果图文数超过10，则将会无响应
        /// </summary>
        public List<ReplyNewsMsgItem> Articles { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", this.ToUserName);
            sb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", this.FromUserName);
            sb.AppendFormat("<CreateTime>{0}</CreateTime>", this.CreateTime);
            sb.Append("<MsgType><![CDATA[news]]></MsgType>");
            sb.AppendFormat("<ArticleCount>{0}</ArticleCount>", this.ArticleCount);
            sb.Append("<Articles>");
            foreach (var item in Articles)
            {
                sb.Append("<item>");
                sb.AppendFormat("<Title><![CDATA[{0}]]></Title>",item.Title);
                sb.AppendFormat("<Description><![CDATA[{0}]]></Description>",item.Description);
                sb.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl>", item.PicUrl);
                sb.AppendFormat("<Url><![CDATA[{0}]]></Url>",item.Url);
                sb.Append("</item>");
            }
            sb.Append("</Articles>");
            sb.Append("</xml>");
            return sb.ToString();
        }
    }
    /// <summary>
    /// 图文消息信息
    /// </summary>
    public class ReplyNewsMsgItem
    {
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图文消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }
    }
}