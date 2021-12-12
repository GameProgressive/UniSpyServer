using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Exception.IRC.Channel
{
    public sealed class IRCBannedFromChanException : IRCChannelException
    {
        public IRCBannedFromChanException(string message, string channelName) : base(message, IRCErrorCode.BannedFromChan, channelName)
        {
        }

        public IRCBannedFromChanException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.BannedFromChan, channelName, innerException)
        {
        }
    }
}