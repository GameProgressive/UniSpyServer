using System;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse.ChatChannelResponse
{
    public class GETKEYReply
    {
        public static string BuildGetKeyReply(ChatUserInfo info, string cookie, string flags)
        {
            return info.BuildReply(ChatReplyCode.GetKey, $"param1 {info.NickName} {cookie} {flags}");
        }
        public static string BuildEndOfGetKeyReply(ChatUserInfo info, string cookie)
        {
            return info.BuildReply(ChatReplyCode.EndGetKey, $"param1 param2 {cookie} param4", "End of GETKEY.");
        }
    }
}
