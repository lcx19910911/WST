using WST.Core.MP.ReplyMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WST.Core.MP.ReplyMsg.Message
{
    /// <summary>
    /// 音乐消息
    /// </summary>
    public class ReplyMusicMsg : ReplyMessage
    {
        /// <summary>
        /// 音乐标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 音乐描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 音乐链接
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl { get; set; }
        /// <summary>
        /// 缩略图的媒体id，通过上传多媒体文件，得到的id
        /// </summary>
        public string ThumbMediaId { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", this.ToUserName);
            sb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", this.FromUserName);
            sb.AppendFormat("<CreateTime>{0}</CreateTime>", this.CreateTime);
            sb.AppendFormat("<MsgType><![CDATA[music]]></MsgType>");
            sb.AppendFormat("<Music>");
            sb.AppendFormat("<Title><![CDATA[{0}]]></Title>",this.Title);
            sb.AppendFormat("<Description><![CDATA[{0}]]></Description>",this.Description);
            sb.AppendFormat("<MusicUrl><![CDATA[{0}]]></MusicUrl>",this.MusicUrl);
            sb.AppendFormat("<HQMusicUrl><![CDATA[{0}]]></HQMusicUrl>",this.HQMusicUrl);
            sb.AppendFormat("<ThumbMediaId><![CDATA[{0}]]></ThumbMediaId>",this.ThumbMediaId);
            sb.AppendFormat("</Music>");
            sb.Append("</xml>");
            return sb.ToString();
        }
    }
}