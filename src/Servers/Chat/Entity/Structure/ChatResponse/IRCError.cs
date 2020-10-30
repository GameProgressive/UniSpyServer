using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatUser;
using System.Collections.Generic;

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
        protected static string BuildChannelError(string ircError, string channelName, string message)
        {
            return ChatReplyBase.BuildReply(ircError, $"* {channelName} param2", message);
        }

        public static string BuildBadChanMaskError(string channelName)
        {
            return BuildChannelError(BadChanMask, channelName, "Bad channel mask.");
        }

        public static string BuildBadChannelKeyError(string channelName)
        {
            return BuildChannelError(BannedFromChan, channelName, "Wrong channel password.");
        }

        public static string BuildBannedFromChannelError(string channelName)
        {
            return BuildChannelError(BadChannelKey, channelName, "Banned from channel.");
        }

        public static string BuildChannelIsFullError(string channelName)
        {
            return BuildChannelError(ChannelIsFull, channelName, "Channel is full.");
        }

        public static string BuildInvitedOnlyChannelError(string channelName)
        {
            return BuildChannelError(InviteOnlyChan, channelName, "Invited only channel.");
        }
        public static string BuildNoSuchChannelError(string channelName)
        {
            return BuildChannelError(NoSuchChannel, channelName, "There is no such channel.");
        }
        public static string BuildToManyChannelError(string channelName)
        {
            return BuildChannelError(TooManyChannels, channelName, "You have joined to many channels.");
        }
        #endregion

        public static string BuildNickNameInUseError(string oldNick, string newNick)
        {
            return ChatReplyBase.BuildReply(NickNameInUse, $"{oldNick} {newNick} 0");
        }

        public static string BuildLoginFailedError()
        {
            return ChatReplyBase.BuildReply(LoginFailed);
        }

        public static string BuildNoUniqueNickError()
        {
            return ChatReplyBase.BuildReply(NoUniqueNick);
        }

        public static string BuildUniquenickExpireError()
        {
            return ChatReplyBase.BuildReply(UniqueNIickExpired);
        }

        public static string BuildRegisterNickFailedError(List<string> nickNames)
        {
            string suggestNicks = "";

            foreach (var nick in nickNames)
            {
                suggestNicks += @"\" + nick;
            }
            return ChatReplyBase.BuildReply(RegisterNickFailed, $"* numberOfSuggestNick {suggestNicks} 0");
        }

        public static string BuildErrOneUSNickNameError(ChatUserInfo info)
        {
            return info.BuildReply(ErrOneUSNickName);
        }

        public static string BuildNoSuchNickError()
        {
            return ChatReplyBase.BuildReply(NoSuchNick);
        }


    }
}
