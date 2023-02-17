using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Exception.IRC.Channel
{
    public sealed class IRCChannelIsFullException : IRCChannelException
    {
        public IRCChannelIsFullException(string message, string channelName) : base(message, IRCErrorCode.ChannelIsFull, channelName){ }

        public IRCChannelIsFullException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.ChannelIsFull, channelName, innerException){ }
    }
}