using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    public class CDKEYRequest : ChatRequestBase
    {
        public CDKEYRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string CDKey { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if (_cmdParams.Count < 1)
                throw new ChatException("The number of IRC cmdParams are incorrect.");
            CDKey = _cmdParams[0];
        }
    }
}
