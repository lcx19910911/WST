using WST.Core.MP.ReplyMsg.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WST.Core.MP.ReplyMsg
{
    #region 被动响应消息类型
    /// <summary>
    /// 被动响应消息类型
    /// </summary>
    public enum Enum_WXReplyMsg_Type
    {
        文本消息=1,
        图片消息=2,
        语音消息=3,
        视频消息=4,
        音乐消息=5,
        图文消息=6
    }
    #endregion 

    /// <summary>
    /// 被动响应消息
    /// </summary>
    public class ReplyMessage
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public Enum_WXReplyMsg_Type MsgType { get; set; }

        #region 消息实体转化为XML字符串
        /// <summary>
        /// 消息实体转化为XML字符串
        /// </summary>
        /// <param name="msg">消息实体</param>
        /// <returns></returns>
        public string Reverse()
        {
            try
            {
                switch (this.MsgType)
                {
                    case Enum_WXReplyMsg_Type.文本消息:
                        return (this as ReplyTextMsg).Reverse();
                    case Enum_WXReplyMsg_Type.图片消息:
                    //    return (this as ReplyPicMsg).Reverse();
                    //case Enum_WXReplyMsg_Type.语音消息:
                    //    return (this as ReplyVoiceMsg).Reverse();
                    //case Enum_WXReplyMsg_Type.视频消息:
                    //    return (this as ReplyVideoMsg).Reverse();
                    //case Enum_WXReplyMsg_Type.音乐消息:
                    //    return (this as ReplyMusicMsg).Reverse();
                    case Enum_WXReplyMsg_Type.图文消息:
                        return (this as ReplyNewsMsg).Reverse();
                    default:
                        return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion
    }
}