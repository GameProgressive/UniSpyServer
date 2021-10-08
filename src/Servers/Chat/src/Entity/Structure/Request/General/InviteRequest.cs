using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("INVITE")]
    internal sealed class InviteRequest : RequestBase
    {
        public InviteRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; private set; }
        public string UserName { get; private set; }

        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count != 2)
            {
                throw new Exception.Exception("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            ChannelName = _cmdParams[0];
            UserName = _cmdParams[1];
        }
    }
}
