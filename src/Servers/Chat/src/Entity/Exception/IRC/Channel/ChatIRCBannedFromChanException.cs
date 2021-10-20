using UniSpyServer.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Chat.Entity.Exception.IRC.Channel
{
    public sealed class ChatIRCBannedFromChanException : IRCChannelException
    {
        public ChatIRCBannedFromChanException(string message, string channelName) : base(message, IRCErrorCode.BannedFromChan, channelName)
        {
        }

        public ChatIRCBannedFromChanException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BannedFromChan, channelName, innerException)
        {
        }
    }
}