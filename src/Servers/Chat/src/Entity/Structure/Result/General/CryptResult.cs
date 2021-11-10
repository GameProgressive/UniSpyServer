using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.General
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
