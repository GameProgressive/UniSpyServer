using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatResultBase : UniSpyResult
    {
        public string IRCErrorCode { get; set; }

        public ChatResultBase()
        {
        }
    }
}
