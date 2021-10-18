using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Result.General
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
