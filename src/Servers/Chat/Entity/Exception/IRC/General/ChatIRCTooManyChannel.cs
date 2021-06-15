using Chat.Entity.Structure.Misc;
namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCTooManyChannels : ChatIRCException
    {
        public ChatIRCTooManyChannels()
        {
        }

        public ChatIRCTooManyChannels(string message) : base(message, ChatIRCErrorCode.TooManyChannels)
        {
        }

        public ChatIRCTooManyChannels(string message, System.Exception innerException) : base(message, ChatIRCErrorCode.TooManyChannels, innerException)
        {
        }
    }
}