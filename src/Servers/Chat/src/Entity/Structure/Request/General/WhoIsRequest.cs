using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("WHOIS")]
    public sealed class WhoIsRequest : RequestBase
    {
        public WhoIsRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; private set; }

        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count != 1)
            {
                throw new Exception.Exception("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            NickName = _cmdParams[0];
        }
    }
}
