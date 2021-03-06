﻿using Chat.Entity.Structure.ChatChannel;

namespace Chat.Entity.Structure.Response.Channel
{
    public class GETCHANKEYReply
    {
        public static string BuildGetChanKeyReply(ChatChannelUser user, string channelName, string cookie, string flags)
        {
            return user.BuildReply(ChatReplyCode.GetChanKey, $"param1 {channelName} {cookie} {flags}");
        }
    }
}
