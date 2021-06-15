using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal sealed class ChatIRCChannelIsFullException : ChatIRCChannelException
    {
        public ChatIRCChannelIsFullException(string message, string channelName) : base(message, ChatIRCErrorCode.ChannelIsFull, channelName)
        {
        }

        public ChatIRCChannelIsFullException(string message, string channelName, System.Exception innerException) : base(message, ChatIRCErrorCode.ChannelIsFull, channelName, innerException)
        {
        }
    }
}