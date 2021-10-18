using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("CDKEY")]
    public sealed class CDKeyRequest : RequestBase
    {
        public CDKeyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string CDKey { get; private set; }

        public override void Parse()
        {
            base.Parse();
            if (_cmdParams.Count < 1)
                throw new Exception.Exception("The number of IRC cmdParams are incorrect.");
            CDKey = _cmdParams[0];
        }
    }
}
