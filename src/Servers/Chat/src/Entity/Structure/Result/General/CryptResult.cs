using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Chat.Entity.Structure.Result.General
{
    public sealed class CryptResult : ResultBase
    {
        public string ServerKey => ChatConstants.ServerKey;
        public string ClientKey => ChatConstants.ClientKey;
        public CryptResult()
        {
        }
    }
}
