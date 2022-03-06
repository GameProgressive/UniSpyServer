using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Exception
{
    public class IRCChannelException : IRCException
    {
        public string ChannelName { get; private set; }
        public override string ErrorResponse => IRCReplyBuilder.BuildChannelError(ErrorCode, ChannelName, Message);
        public IRCChannelException(){ }

        public IRCChannelException(string message, string errorCode, string channelName) : base(message, errorCode)
        {
            ChannelName = channelName;
        }

        public IRCChannelException(string message, string errorCode, string channelName, System.Exception innerException) : base(message, errorCode, innerException)
        {
            ChannelName = channelName;
        }
    }
}