using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class CRYPTResult : ChatResultBase
    {
        public string ServerKey { get; }
        public string ClientKey { get; }
        public CRYPTResult()
        {
            ServerKey = ChatConstants.ServerKey;
            ClientKey = ChatConstants.ClientKey;
        }
    }
}
