﻿using System;
using System.Collections.Generic;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse
{
    public class ChatIRCError
    {
        //irc standard error
        public const string NoSuchNick = "401";
        public const string NoSuchChannel = "403";
        public const string TooManyChannels = "405";
        public const string ErrOneUSNickName = "432";
        public const string NickNameInUse = "433";
        public const string MoreParameters = "461";
        public const string ChannelIsFull = "471";
        public const string InviteOnlyChan = "473";
        public const string BannedFromChan = "474";
        public const string BadChannelKey = "475";
        public const string BadChanMask = "476";
        public const string LoginFailed = "708";
        public const string NoUniqueNick = "709";
        public const string UniqueNIickExpired = "710";
        public const string RegisterNickFailed = "711";


        #region channel error RPL
        protected static string BuildChannelError(string ircError, string channelName)
        {
            return ChatCommandBase.BuildReply(ircError, $"* {channelName} 0");
        }
        public static string BuildBadChanMaskError(string channelName)
        {
            return BuildChannelError(ChatIRCError.BadChanMask, channelName);
        }

        public static string BuildBadChannelKeyError(string channelName)
        {
            return BuildChannelError(ChatIRCError.BannedFromChan, channelName);
        }

        public static string BuildBannedFromChannelError(string channelName)
        {
            return BuildChannelError(ChatIRCError.BadChannelKey, channelName);
        }

        public static string BuildChannelIsFullError(string channelName)
        {
            return BuildChannelError(ChatIRCError.ChannelIsFull, channelName);
        }

        public static string BuildInvitedOnlyChannelError(string channelName)
        {
            return BuildChannelError(ChatIRCError.InviteOnlyChan, channelName);
        }
        public static string BuildNoSuchChannelError(string channelName)
        {
            return BuildChannelError(ChatIRCError.NoSuchChannel, channelName);
        }
        #endregion

        public static string BuildNickNameInUseError(string oldNick, string newNick)
        {
            return ChatCommandBase.BuildReply(ChatIRCError.NickNameInUse, $"{oldNick} {newNick} 0");
        }

        public static string BuildLoginFailedError()
        {
            return ChatCommandBase.BuildReply(ChatIRCError.LoginFailed);
        }

        public static string BuildNoUniqueNickError()
        {
            return ChatCommandBase.BuildReply(ChatIRCError.NoUniqueNick);
        }

        public static string BuildUniquenickExpireError()
        {
            return ChatCommandBase.BuildReply(ChatIRCError.UniqueNIickExpired);
        }

        public static string BuildRegisterNickFailedError(List<string> nickNames)
        {
            string suggestNicks = "";

            foreach (var nick in nickNames)
            {
                suggestNicks += @"\" + nick;
            }
            return ChatCommandBase.BuildReply(ChatIRCError.RegisterNickFailed, $"* numberOfSuggestNick {suggestNicks} 0");
        }

        public static string BuildErrOneUSNickNameError(ChatUserInfo info)
        {
            return info.BuildReply(ChatIRCError.ErrOneUSNickName);
        }

        public static string BuildNoSuchNickError()
        {
            return ChatCommandBase.BuildReply(ChatIRCError.NoSuchNick);
        }




    }
}