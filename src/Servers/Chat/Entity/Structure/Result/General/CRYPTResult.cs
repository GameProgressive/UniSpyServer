using Chat.Abstraction.BaseClass;
using Chat.Network;

namespace Chat.Entity.Structure.Result.General
{
    internal class CRYPTResult : ChatResultBase
    {
        public string ServerKey { get; set; }
        public string ClientKey { get; set; }
        public CRYPTResult()
        {
            ServerKey = ChatServer.ServerKey;
            ClientKey = ChatServer.ClientKey;
        }
    }
}
