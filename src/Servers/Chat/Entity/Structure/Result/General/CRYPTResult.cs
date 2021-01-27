using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result
{
    internal class CRYPTResult : ChatResultBase
    {
        public string ServerKey { get; protected set; }
        public string ClientKey { get; protected set; }
        public CRYPTResult(string clientKey, string serverKey)
        {
            ServerKey = serverKey;
            ClientKey = clientKey;
        }
    }
}
