using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception
{
    public class ChatIRCChannelException : ChatIRCException
    {
        public string ChannelName { get; private set; }
        public override string ErrorResponse => ChatIRCReplyBuilder.BuildChannelError(ErrorCode, ChannelName, Message);
        public ChatIRCChannelException()
        {
        }

        public ChatIRCChannelException(string message, string errorCode, string channelName) : base(message, errorCode)
        {
            ChannelName = channelName;
        }

        public ChatIRCChannelException(string message, string errorCode, string channelName, System.Exception innerException) : base(message, errorCode, innerException)
        {
            ChannelName = channelName;
        }
    }
}