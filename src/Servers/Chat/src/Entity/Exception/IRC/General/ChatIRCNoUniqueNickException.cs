using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Exception.IRC.General
{
    public sealed class ChatIRCNoUniqueNickException : IRCException
    {
        public ChatIRCNoUniqueNickException()
        {
        }

        public ChatIRCNoUniqueNickException(string message) : base(message, IRCErrorCode.NoUniqueNick)
        {
        }

        public ChatIRCNoUniqueNickException(string message, System.Exception innerException) : base(message, IRCErrorCode.NoUniqueNick, innerException)
        {
        }
    }
}