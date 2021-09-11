using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal sealed class ChatIRCChannelIsFullException : IRCChannelException
    {
        public ChatIRCChannelIsFullException(string message, string channelName) : base(message, IRCErrorCode.ChannelIsFull, channelName)
        {
        }

        public ChatIRCChannelIsFullException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.ChannelIsFull, channelName, innerException)
        {
        }
    }
}