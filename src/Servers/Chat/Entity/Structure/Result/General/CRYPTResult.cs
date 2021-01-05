using System;
using Chat.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result
{
    public class CRYPTResult : ChatResultBase
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
