using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    public class SETGROUPRequest : ChatChannelRequestBase
    {
        public SETGROUPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string GroupName { get; protected set; }
        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count != 1)
            {
                throw new ChatException("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            GroupName = _cmdParams[0];
        }
    }
}
