using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal sealed class ChatIRCBannedFromChanException : IRCChannelException
    {
        public ChatIRCBannedFromChanException(string message, string channelName) : base(message, IRCErrorCode.BannedFromChan, channelName)
        {
        }

        public ChatIRCBannedFromChanException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BannedFromChan, channelName, innerException)
        {
        }
    }
}