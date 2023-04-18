using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.Channel
{
    public sealed class ChannelIsFullException : IRCChannelException
    {
        public ChannelIsFullException(string message, string channelName) : base(message, IRCErrorCode.ChannelIsFull, channelName){ }

        public ChannelIsFullException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.ChannelIsFull, channelName, innerException){ }
    }
}