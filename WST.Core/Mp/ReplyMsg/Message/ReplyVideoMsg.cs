using WST.Core.MP.ReplyMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WST.Core.MP.ReplyMsg.Message
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class ReplyVideoMsg:ReplyMessage
    {
        /// <summary>
        /// 通过上传多媒体文件，得到的id
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 视频消息的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 视频消息的描述
        /// </summary>
        public string Description { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", this.ToUserName);
            sb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", this.FromUserName);
            sb.AppendFormat("<CreateTime>{0}</CreateTime>", this.CreateTime);
            sb.AppendFormat("<MsgType><![CDATA[video]]></MsgType>");
            sb.Append("<Video>");
            sb.AppendFormat("<MediaId><![CDATA[{0}]]></MediaId>", this.MediaId);
            sb.AppendFormat("<Title><![CDATA[{0}]]></Title>",this.Title);
            sb.AppendFormat("<Description><![CDATA[{0}]]></Description>",this.Description);
            sb.Append("</Video>");
            sb.Append("</xml>");
            return sb.ToString();
        }
    }
}