using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal sealed class IRCBadChanMaskException : IRCChannelException
    {
        public IRCBadChanMaskException()
        {
        }

        public IRCBadChanMaskException(string message, string channelName) : base(message, IRCErrorCode.BadChanMask, channelName)
        {
        }

        public IRCBadChanMaskException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BadChanMask, channelName, innerException)
        {
        }
    }
}