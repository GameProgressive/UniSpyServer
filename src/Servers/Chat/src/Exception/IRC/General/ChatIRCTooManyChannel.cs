using UniSpy.Server.Chat.Aggregate.Misc;
namespace UniSpy.Server.Chat.Exception.IRC.General
{
    public sealed class ChatIRCTooManyChannels : IRCException
    {
        public ChatIRCTooManyChannels(){ }

        public ChatIRCTooManyChannels(string message) : base(message, IRCErrorCode.TooManyChannels){ }

        public ChatIRCTooManyChannels(string message, System.Exception innerException) : base(message, IRCErrorCode.TooManyChannels, innerException){ }
    }
}