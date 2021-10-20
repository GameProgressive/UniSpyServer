using UniSpyServer.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Chat.Entity.Exception.IRC.Channel
{
    public sealed class ChatIRCChannelIsFullException : IRCChannelException
    {
        public ChatIRCChannelIsFullException(string message, string channelName) : base(message, IRCErrorCode.ChannelIsFull, channelName)
        {
        }

        public ChatIRCChannelIsFullException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.ChannelIsFull, channelName, innerException)
        {
        }
    }
}