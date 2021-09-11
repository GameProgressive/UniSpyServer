using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal sealed class ChatIRCInviteOnlyChanException : IRCChannelException
    {
        public ChatIRCInviteOnlyChanException()
        {
        }

        public ChatIRCInviteOnlyChanException(string message, string channelName) : base(message, IRCErrorCode.InviteOnlyChan, channelName)
        {
        }

        public ChatIRCInviteOnlyChanException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.InviteOnlyChan, channelName, innerException)
        {
        }
    }
}