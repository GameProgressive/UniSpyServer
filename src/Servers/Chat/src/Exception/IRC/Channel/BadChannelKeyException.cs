using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.Channel
{
    public sealed class BadChannelKeyException : IRCChannelException
    {
        public BadChannelKeyException(){ }

        public BadChannelKeyException(string message, string channelName) : base(message, IRCErrorCode.BadChannelKey, channelName){ }

        public BadChannelKeyException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BadChannelKey, channelName, innerException){ }
    }
}