using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Exception.IRC.Channel
{
    public sealed class ChatIRCInviteOnlyChanException : IRCChannelException
    {
        public ChatIRCInviteOnlyChanException()
        {
        }

        public ChatIRCInviteOnlyChanException(string message, string channelName) : base(message, IRCErrorCode.InviteOnlyChan, channelName)
        {
        }

        public ChatIRCInviteOnlyChanException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.InviteOnlyChan, channelName, innerException)
        {
        }
    }
}