using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal class ChatIRCInviteOnlyChanException : ChatIRCChannelException
    {
        public ChatIRCInviteOnlyChanException()
        {
        }

        public ChatIRCInviteOnlyChanException(string message, string channelName) : base(message, ChatIRCErrorCode.InviteOnlyChan, channelName)
        {
        }

        public ChatIRCInviteOnlyChanException(string message, string channelName, System.Exception innerException) : base(message, ChatIRCErrorCode.InviteOnlyChan, channelName, innerException)
        {
        }
    }
}