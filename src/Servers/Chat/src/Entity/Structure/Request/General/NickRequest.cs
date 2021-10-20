using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;

namespace UniSpyServer.Chat.Entity.Structure.Request.General
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
            }
            else if (_longParam != null)
            {
                NickName = _longParam;
            }
            else
            {
                throw new Exception.Exception("NICK request is invalid.");
            }
        }
    }
}
