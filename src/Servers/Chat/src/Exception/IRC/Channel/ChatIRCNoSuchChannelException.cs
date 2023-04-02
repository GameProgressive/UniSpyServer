using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.Channel
{
    public sealed class ChatIRCNoSuchChannelException : IRCChannelException
    {
        public ChatIRCNoSuchChannelException(){ }

        public ChatIRCNoSuchChannelException(string message, string channelName) : base(message, IRCErrorCode.NoSuchChannel, channelName){ }

        public ChatIRCNoSuchChannelException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.NoSuchChannel, channelName, innerException){ }
    }
}