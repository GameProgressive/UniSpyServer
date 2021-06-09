using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Result.General
{
    internal class CRYPTResult : ChatResultBase
    {
        public string ServerKey { get; set; }
        public string ClientKey { get; set; }
        public CRYPTResult()
        {
            ServerKey = ChatConstants.ServerKey;
            ClientKey = ChatConstants.ClientKey;
        }
    }
}
