using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.Channel
{
    public sealed class ChatIRCBadChannelKeyException : IRCChannelException
    {
        public ChatIRCBadChannelKeyException(){ }

        public ChatIRCBadChannelKeyException(string message, string channelName) : base(message, IRCErrorCode.BadChannelKey, channelName){ }

        public ChatIRCBadChannelKeyException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BadChannelKey, channelName, innerException){ }
    }
}