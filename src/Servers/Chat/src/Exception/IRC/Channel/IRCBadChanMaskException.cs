using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.Channel
{
    public sealed class IRCBadChanMaskException : IRCChannelException
    {
        public IRCBadChanMaskException(){ }

        public IRCBadChanMaskException(string message, string channelName) : base(message, IRCErrorCode.BadChanMask, channelName){ }

        public IRCBadChanMaskException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BadChanMask, channelName, innerException){ }
    }
}