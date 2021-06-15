using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal sealed class ChatIRCBannedFromChanException : ChatIRCChannelException
    {
        public ChatIRCBannedFromChanException(string message, string channelName) : base(message, ChatIRCErrorCode.BannedFromChan, channelName)
        {
        }

        public ChatIRCBannedFromChanException(string message, string channelName, System.Exception innerException) : base(message, ChatIRCErrorCode.BannedFromChan, channelName, innerException)
        {
        }
    }
}