using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    internal sealed class INVITERequest : ChatRequestBase
    {
        public INVITERequest(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; private set; }
        public string UserName { get; private set; }

        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count != 2)
            {
                throw new ChatException("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            ChannelName = _cmdParams[0];
            UserName = _cmdParams[1];
        }
    }
}
