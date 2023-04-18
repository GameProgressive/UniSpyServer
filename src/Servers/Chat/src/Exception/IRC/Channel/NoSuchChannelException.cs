using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.Channel
{
    public sealed class NoSuchChannelException : IRCChannelException
    {
        public NoSuchChannelException(){ }

        public NoSuchChannelException(string message, string channelName) : base(message, IRCErrorCode.NoSuchChannel, channelName){ }

        public NoSuchChannelException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.NoSuchChannel, channelName, innerException){ }
    }
}