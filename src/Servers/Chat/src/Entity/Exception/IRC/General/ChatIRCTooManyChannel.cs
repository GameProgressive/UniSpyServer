using UniSpyServer.Chat.Entity.Structure.Misc;
namespace UniSpyServer.Chat.Entity.Exception.IRC.General
{
    public sealed class ChatIRCTooManyChannels : IRCException
    {
        public ChatIRCTooManyChannels()
        {
        }

        public ChatIRCTooManyChannels(string message) : base(message, IRCErrorCode.TooManyChannels)
        {
        }

        public ChatIRCTooManyChannels(string message, System.Exception innerException) : base(message, IRCErrorCode.TooManyChannels, innerException)
        {
        }
    }
}