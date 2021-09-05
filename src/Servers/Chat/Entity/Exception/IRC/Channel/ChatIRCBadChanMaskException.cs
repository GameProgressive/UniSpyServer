using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal sealed class ChatIRCBadChanMaskException : ChatIRCChannelException
    {
        public ChatIRCBadChanMaskException()
        {
        }

        public ChatIRCBadChanMaskException(string message, string channelName) : base(message, ChatIRCErrorCode.BadChanMask, channelName)
        {
        }

        public ChatIRCBadChanMaskException(string message, string channelName, System.Exception innerException) : base(message, ChatIRCErrorCode.BadChanMask, channelName, innerException)
        {
        }
    }
}