using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal class ChatIRCNoSuchChannelException : ChatIRCChannelException
    {
        public ChatIRCNoSuchChannelException()
        {
        }

        public ChatIRCNoSuchChannelException(string message, string channelName) : base(message, ChatIRCErrorCode.NoSuchChannel, channelName)
        {
        }

        public ChatIRCNoSuchChannelException(string message, string channelName, System.Exception innerException) : base(message, ChatIRCErrorCode.NoSuchChannel, channelName, innerException)
        {
        }
    }
}