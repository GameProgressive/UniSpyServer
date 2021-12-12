using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Exception.IRC.Channel
{
    public sealed class IRCChannelIsFullException : IRCChannelException
    {
        public IRCChannelIsFullException(string message, string channelName) : base(message, IRCErrorCode.ChannelIsFull, channelName)
        {
        }

        public IRCChannelIsFullException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.ChannelIsFull, channelName, innerException)
        {
        }
    }
}