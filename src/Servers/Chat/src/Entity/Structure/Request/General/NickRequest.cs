using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("NICK")]
    public sealed class NickRequest : RequestBase
    {
        public NickRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams != null)
            {
                NickName = _cmdParams[0];
                return;
            }

            if (_longParam != null)
            {
                NickName = _longParam;
                return;
            }
            
            throw new Exception.Exception("NICK request is invalid.");
        }
    }
}
