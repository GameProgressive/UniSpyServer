using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Exception
{
    public class IRCChannelException : IRCException
    {
        public string ChannelName { get; private set; }
        public IRCChannelException() { }
        public IRCChannelException(string message, string errorCode, string channelName) : base(message, errorCode)
        {
            ChannelName = channelName;
        }
        public IRCChannelException(string message, string errorCode, string channelName, System.Exception innerException) : base(message, errorCode, innerException)
        {
            ChannelName = channelName;
        }
        public override void Build()
        {
            SendingBuffer = $":{ResponseBase.ServerDomain} {ErrorCode} * {ChannelName} :{Message}\r\n";
        }
    }
}