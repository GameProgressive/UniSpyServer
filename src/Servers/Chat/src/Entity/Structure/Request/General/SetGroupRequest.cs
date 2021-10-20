using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;

namespace UniSpyServer.Chat.Entity.Structure.Request.General
{
    [RequestContract("SETGROUP")]
    public sealed class SetGroupRequest : ChannelRequestBase
    {
        public SetGroupRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string GroupName { get; private set; }
        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count != 1)
            {
                throw new Exception.Exception("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            GroupName = _cmdParams[0];
        }
    }
}
