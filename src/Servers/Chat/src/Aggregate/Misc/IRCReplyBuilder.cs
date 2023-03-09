// using System.Collections.Generic;
// using UniSpy.Server.Chat.Abstraction.BaseClass;

// namespace UniSpy.Server.Chat.Aggregate.Misc
// {
//     public sealed class IRCReplyBuilder
//     {
//         #region advance irc reply builder

//         public static string BuildChannelError(string ircError, string channelName, string message)
//         {
//             return Build(ircError, $"* {channelName}", message);
//         }

//         public static string BuildBadChanMaskError(string channelName)
//         {
//             return BuildChannelError(IRCErrorCode.BadChanMask, channelName, "Bad channel mask.");
//         }

//         public static string BuildBadChannelKeyError(string channelName)
//         {
//             return BuildChannelError(IRCErrorCode.BannedFromChan, channelName, "Wrong channel password.");
//         }

//         public static string BuildBannedFromChannelError(string channelName)
//         {
//             return BuildChannelError(IRCErrorCode.BadChannelKey, channelName, "Banned from channel.");
//         }

//         public static string BuildChannelIsFullError(string channelName)
//         {
//             return BuildChannelError(IRCErrorCode.ChannelIsFull, channelName, "Channel is full.");
//         }

//         public static string BuildInvitedOnlyChannelError(string channelName)
//         {
//             return BuildChannelError(IRCErrorCode.InviteOnlyChan, channelName, "Invite only channel.");
//         }
//         public static string BuildNoSuchChannelError(string channelName)
//         {
//             return BuildChannelError(IRCErrorCode.NoSuchChannel, channelName, "There is no such channel.");
//         }
//         public static string BuildToManyChannelError(string channelName)
//         {
//             return BuildChannelError(IRCErrorCode.TooManyChannels, channelName, "You have joined to many channels.");
//         }

//         public static string BuildNickNameInUseError(string oldNick, string newNick)
//         {
//             return Build(IRCErrorCode.NickNameInUse, $"{oldNick} {newNick} 0");
//         }

//         public static string BuildLoginFailedError()
//         {
//             return Build(IRCErrorCode.LoginFailed);
//         }

//         public static string BuildNoUniqueNickError()
//         {
//             return Build(IRCErrorCode.NoUniqueNick);
//         }

//         public static string BuildUniquenickExpireError()
//         {
//             return Build(IRCErrorCode.UniqueNIickExpired);
//         }

//         public static string BuildRegisterNickFailedError(List<string> nickNames)
//         {
//             string suggestNicks = "";

//             foreach (var nick in nickNames)
//             {
//                 suggestNicks += @"\" + nick;
//             }
//             return Build(IRCErrorCode.RegisterNickFailed, $"* numberOfSuggestNick {suggestNicks} 0");
//         }

//         public static string BuildErrOneUSNickNameError(string userPrefix)
//         {
//             return Build(userPrefix, IRCErrorCode.ErrOneUSNickName, null, null);
//         }

//         public static string BuildNoSuchNickError()
//         {
//             return Build(IRCErrorCode.NoSuchNick);
//         }
//         #endregion

//         #region Basic IRC reply builder
//         public static string Build(string command)
//         {
//             return Build(command, null);
//         }

//         public static string Build(string cmd, string cmdParams)
//         {
//             return Build(cmd, cmdParams, null);
//         }

//         public static string Build(string cmd, string cmdParams, string tailing)
//         {
//             return Build(ResponseBase.ServerDomain, cmd, cmdParams, tailing);
//         }
//         public static string Build(IRCErrorCode cmd, string cmdParams, string tailing)
//         {
//             return Build(cmd.ToString(), cmdParams, tailing);
//         }
//         public static string Build(string prefix, string cmd, string cmdParams, string tailing)
//         {
//             string buffer = "";

//             if (prefix != "" || prefix is not null)
//             {
//                 buffer = $":{prefix} ";
//             }

//             buffer += $"{cmd} {cmdParams}";

//             if (tailing == "" || tailing is null)
//             {
//                 buffer += "\r\n";
//             }
//             else
//             {
//                 buffer += $" :{tailing}\r\n";
//             }

//             return buffer;
//         }
//         #endregion
//     }
// }
