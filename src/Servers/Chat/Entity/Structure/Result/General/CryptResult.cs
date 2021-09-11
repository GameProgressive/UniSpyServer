using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class CryptResult : ResultBase
    {
        public string ServerKey => ChatConstants.ServerKey;
        public string ClientKey => ChatConstants.ClientKey;
        public CryptResult()
        {
        }
    }
}
