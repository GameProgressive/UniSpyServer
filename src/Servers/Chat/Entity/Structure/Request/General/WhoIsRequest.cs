using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("WHOIS")]
    internal sealed class WhoIsRequest : RequestBase
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
