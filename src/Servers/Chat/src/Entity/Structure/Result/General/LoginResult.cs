using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.General
{
    public sealed class LoginResult : ResultBase
    {
        public uint ProfileID { get; set; }
        public uint UserID { get; set; }
        public LoginResult()
        {
        }
    }
}
