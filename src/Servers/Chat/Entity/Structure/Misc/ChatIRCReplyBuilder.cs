using System;
using System.Collections.Generic;
using Chat.Network;

namespace Chat.Entity.Structure.Misc
{
    public class ChatIRCReplyBuilder
    {
        public static string BuildByIRCErrorCode(string ircErrorCode, params object[] args)
        {
            switch (ircErrorCode)
            {
                case ChatIRCErrorCode.NoSuchNick:
                    return BuildNoSuchNickError();
                case ChatIRCErrorCode.NoSuchChannel:
                    return BuildNoSuchChannelError((string)args[0]);
                case ChatIRCErrorCode.TooManyChannels:
                    return BuildToManyChannelError((string)args[0]);
                case ChatIRCErrorCode.ErrOneUSNickName:
                    return BuildErrOneUSNickNameError((string)args[0]);
                case ChatIRCErrorCode.NickNameInUse:
                    return BuildNickNameInUseError((string)args[0], (string)args[1]);
                case ChatIRCErrorCode.MoreParameters:
                    throw new NotImplementedException();
                case ChatIRCErrorCode.ChannelIsFull:
                    return BuildChannelIsFullError((string)args[0]);
                case ChatIRCErrorCode.InviteOnlyChan:
                    return BuildInvitedOnlyChannelError((string)args[0]);
                case ChatIRCErrorCode.BannedFromChan:
                    return BuildBannedFromChannelError((string)args[0]);
                case ChatIRCErrorCode.BadChannelKey:
                    return BuildBadChannelKeyError((string)args[0]);
                case ChatIRCErrorCode.BadChanMask:
                    return BuildBadChanMaskError((string)args[0]);
                case ChatIRCErrorCode.LoginFailed:
                    return BuildLoginFailedError();
                case ChatIRCErrorCode.NoUniqueNick:
                    return BuildNoUniqueNickError();
                case ChatIRCErrorCode.UniqueNIickExpired:
                    return BuildUniquenickExpireError();
                case ChatIRCErrorCode.RegisterNickFailed:
                    return BuildRegisterNickFailedError((List<string>)args[0]);
                default:
                    throw new ArgumentException("Unknown IRC error code");
            }
        }

        #region advance irc reply builder
        private static string BuildChannelError(string ircError, string channelName, string message)
        {
            return Build(ircError, $"* {channelName} param2", message);
        }

        public static string BuildBadChanMaskError(string channelName)
        {
            return BuildChannelError(ChatIRCErrorCode.BadChanMask, channelName, "Bad channel mask.");
        }

        public static string BuildBadChannelKeyError(string channelName)
        {
            return BuildChannelError(ChatIRCErrorCode.BannedFromChan, channelName, "Wrong channel password.");
        }

        public static string BuildBannedFromChannelError(string channelName)
        {
            return BuildChannelError(ChatIRCErrorCode.BadChannelKey, channelName, "Banned from channel.");
        }

        public static string BuildChannelIsFullError(string channelName)
        {
            return BuildChannelError(ChatIRCErrorCode.ChannelIsFull, channelName, "Channel is full.");
        }

        public static string BuildInvitedOnlyChannelError(string channelName)
        {
            return BuildChannelError(ChatIRCErrorCode.InviteOnlyChan, channelName, "Invited only channel.");
        }
        public static string BuildNoSuchChannelError(string channelName)
        {
            return BuildChannelError(ChatIRCErrorCode.NoSuchChannel, channelName, "There is no such channel.");
        }
        public static string BuildToManyChannelError(string channelName)
        {
            return BuildChannelError(ChatIRCErrorCode.TooManyChannels, channelName, "You have joined to many channels.");
        }

        public static string BuildNickNameInUseError(string oldNick, string newNick)
        {
            return Build(ChatIRCErrorCode.NickNameInUse, $"{oldNick} {newNick} 0");
        }

        public static string BuildLoginFailedError()
        {
            return Build(ChatIRCErrorCode.LoginFailed);
        }

        public static string BuildNoUniqueNickError()
        {
            return Build(ChatIRCErrorCode.NoUniqueNick);
        }

        public static string BuildUniquenickExpireError()
        {
            return Build(ChatIRCErrorCode.UniqueNIickExpired);
        }

        public static string BuildRegisterNickFailedError(List<string> nickNames)
        {
            string suggestNicks = "";

            foreach (var nick in nickNames)
            {
                suggestNicks += @"\" + nick;
            }
            return Build(ChatIRCErrorCode.RegisterNickFailed, $"* numberOfSuggestNick {suggestNicks} 0");
        }

        public static string BuildErrOneUSNickNameError(string userPrefix)
        {
            return Build(userPrefix, ChatIRCErrorCode.ErrOneUSNickName, null, null);
        }

        public static string BuildNoSuchNickError()
        {
            return Build(ChatIRCErrorCode.NoSuchNick);
        }
        #endregion

        #region Basic IRC reply builder
        public static string Build(string command)
        {
            return Build(command, null);
        }

        public static string Build(string command, string cmdParams)
        {
            return Build(command, cmdParams, null);
        }

        public static string Build(string command, string cmdParams, string tailing)
        {
            return Build(ChatServer.ServerDomain, command, cmdParams, tailing);
        }

        public static string Build(string prefix, string cmd, string cmdParams, string tailing)
        {
            string buffer = "";

            if (prefix != "" || prefix != null)
            {
                buffer = $":{prefix} ";
            }

            buffer += $"{cmd} {cmdParams}";

            if (tailing != "" || tailing != null)
            {
                buffer += $" :{tailing}\r\n";
            }
            else
            {
                buffer += "\r\n";
            }

            return buffer;
        }
        #endregion
    }
}
