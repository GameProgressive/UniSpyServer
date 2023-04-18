using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.Channel
{
    public sealed class BadChanMaskException : IRCChannelException
    {
        public BadChanMaskException(){ }

        public BadChanMaskException(string message, string channelName) : base(message, IRCErrorCode.BadChanMask, channelName){ }

        public BadChanMaskException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BadChanMask, channelName, innerException){ }
    }
}