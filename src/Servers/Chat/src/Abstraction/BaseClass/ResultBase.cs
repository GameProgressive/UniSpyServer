using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Abstraction.BaseClass
{
    public abstract class ResultBase : UniSpyResultBase
    {
        public string IRCErrorCode { get; set; }

        public ResultBase()
        {
        }
    }
}
