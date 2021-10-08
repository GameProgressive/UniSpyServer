using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ResultBase : UniSpyResultBase
    {
        public string IRCErrorCode { get; set; }

        public ResultBase()
        {
        }
    }
}
