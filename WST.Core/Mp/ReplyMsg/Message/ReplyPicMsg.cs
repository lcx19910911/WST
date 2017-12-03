using WST.Core.MP.ReplyMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace WST.Core.MP.ReplyMsg.Message
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class ReplyPicMsg : ReplyMessage
    {
        /// <summary>
        /// 通过上传多媒体文件，得到的id
        /// </summary>
        public string MediaId { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", this.ToUserName);
            sb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", this.FromUserName);
            sb.AppendFormat("<CreateTime>{0}</CreateTime>",this.CreateTime);
            sb.AppendFormat("<MsgType><![CDATA[image]]></MsgType>");
            sb.Append("<Image>");
            sb.AppendFormat("<MediaId><![CDATA[{0}]]></MediaId>",this.MediaId);
            sb.Append("</Image>");
            sb.Append("</xml>");
            return sb.ToString();
        }
    }
}