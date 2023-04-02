using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.Channel
{
    public sealed class IRCBannedFromChanException : IRCChannelException
    {
        public IRCBannedFromChanException(string message, string channelName) : base(message, IRCErrorCode.BannedFromChan, channelName){ }

        public IRCBannedFromChanException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BannedFromChan, channelName, innerException){ }
    }
}