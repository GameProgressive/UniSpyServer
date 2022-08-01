using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Exception.IRC.General
{
    public sealed class ChatIRCNoSuchNickException : IRCException
    {
        public ChatIRCNoSuchNickException(){ }

        public ChatIRCNoSuchNickException(string message) : base(message, IRCErrorCode.NoSuchNick){ }

        public ChatIRCNoSuchNickException(string message, System.Exception innerException) : base(message, IRCErrorCode.NoSuchNick, innerException){ }
    }
}