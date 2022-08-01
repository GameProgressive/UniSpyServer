using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Abstraction
{
    public abstract class LoginResultBase : ResultBase
    {
        public int? ResponseCode { get; set; }
        public int Length { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public string ProfileNick { get; set; }
        public string UniqueNick { get; set; }
        public string CdKeyHash { get; set; }
        public LoginResultBase()
        {
            ResponseCode = 0;
            Length = 303;
        }
    }
}