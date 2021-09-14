using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("NICK")]
    internal sealed class NickRequest : RequestBase
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
