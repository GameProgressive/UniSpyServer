﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.General
{
    public class USRIPReply
    {
        public static string BuildUserIPReply(string ip)
        {
            return ChatResponseBase.BuildRPL(ChatReplyName.UserIP, "", $"@{ip}");
        }
    }
}
