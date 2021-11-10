using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Exception.IRC.Channel
{
    public sealed class ChatIRCBadChannelKeyException : IRCChannelException
    {
        public ChatIRCBadChannelKeyException()
        {
        }

        public ChatIRCBadChannelKeyException(string message, string channelName) : base(message, IRCErrorCode.BadChannelKey, channelName)
        {
        }

        public ChatIRCBadChannelKeyException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BadChannelKey, channelName, innerException)
        {
        }
    }
}