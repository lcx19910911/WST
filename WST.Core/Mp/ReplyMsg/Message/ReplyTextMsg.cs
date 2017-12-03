using WST.Core.MP.ReplyMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WST.Core.MP.ReplyMsg.Message
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class ReplyTextMsg:ReplyMessage
    {
        /// <summary>
        /// 回复的消息内容
        /// </summary>
        public string Content { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>",this.ToUserName);
            sb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>",this.FromUserName);
            sb.AppendFormat("<CreateTime>{0}</CreateTime>", this.CreateTime);
            sb.AppendFormat("<MsgType><![CDATA[text]]></MsgType>");
            sb.AppendFormat("<Content><![CDATA[{0}]]></Content>",this.Content);
            sb.Append("</xml>");
            return sb.ToString();
        }
    }
}