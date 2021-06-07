using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.Channel
{
    internal class ChatIRCBadChannelKeyException : ChatIRCChannelException
    {
        public ChatIRCBadChannelKeyException()
        {
        }

        public ChatIRCBadChannelKeyException(string message, string channelName) : base(message, ChatIRCErrorCode.BadChannelKey, channelName)
        {
        }

        public ChatIRCBadChannelKeyException(string message, string channelName, System.Exception innerException) : base(message, ChatIRCErrorCode.BadChannelKey, channelName, innerException)
        {
        }
    }
}