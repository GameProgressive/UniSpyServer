using System;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Handler.SystemHandler.ChatErrorMessage
{
    public class ChatErrorMessageBuilder
    {
        public static string GetErrorMessage(string errorCode)
        {
            string buffer = "";
            switch (errorCode)
            {
                case IRCError.BadChanMask:
                    buffer = "Channel mask is wrong!";
                    break;
                case IRCError.BadChannelKey:
                    buffer = "Channel password is wrong!";
                    break;
                case IRCError.BannedFromChan:
                    buffer = "You are banned from this channel!";
                    break;
                case IRCError.ChannelIsFull:
                    buffer = "Channel is full!";
                    break;
                case IRCError.ErrOneUSNickName:
                    buffer = "Nick name is not correct";
                    break;
                case IRCError.InviteOnlyChan:
                    buffer = "You are not been invited to this channel";
                    break;
                case IRCError.MoreParameters:

                    break;
                case IRCError.LoginFailed:
                case IRCError.NickNameInUse:
                case IRCError.NoSuchChannel:
                case IRCError.NoSuchNick:
                case IRCError.NoUniqueNick:
                case IRCError.RegisterNickFailed:
                case IRCError.TooManyChannels:
                case IRCError.UniqueNIickExpired:
                    break;
            }
            return buffer;
        }

      
    }
}
