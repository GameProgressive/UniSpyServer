using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.Channel
{
    public sealed class InviteOnlyChanException : IRCChannelException
    {
        public InviteOnlyChanException(){ }

        public InviteOnlyChanException(string message, string channelName) : base(message, IRCErrorCode.InviteOnlyChan, channelName){ }

        public InviteOnlyChanException(string message, string channelName, System.Exception innerException) : base(message, IRCErrorCode.InviteOnlyChan, channelName, innerException){ }
    }
}