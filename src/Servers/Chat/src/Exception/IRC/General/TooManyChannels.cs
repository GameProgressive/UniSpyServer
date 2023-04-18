using UniSpy.Server.Chat.Aggregate.Misc;
namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class TooManyChannels : IRCException
    {
        public TooManyChannels(){ }

        public TooManyChannels(string message) : base(message, IRCErrorCode.TooManyChannels){ }

        public TooManyChannels(string message, System.Exception innerException) : base(message, IRCErrorCode.TooManyChannels, innerException){ }
    }
}