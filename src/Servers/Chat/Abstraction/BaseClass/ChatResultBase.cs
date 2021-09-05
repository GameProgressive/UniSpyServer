using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatResultBase : UniSpyResultBase
    {
        public string IRCErrorCode { get; set; }

        public ChatResultBase()
        {
        }
    }
}
