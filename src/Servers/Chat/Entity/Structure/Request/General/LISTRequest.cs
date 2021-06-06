using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    public class LISTRequest : ChatRequestBase
    {
        public LISTRequest(string rawRequest) : base(rawRequest)
        {
        }

        public bool IsSearchingChannel { get; protected set; }
        public bool IsSearchingUser { get; protected set; }
        public string Filter { get; protected set; }

        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count == 0)
            {
                throw new ChatException("Search filter is missing.");
            }
            IsSearchingChannel = true;
            Filter = _cmdParams[0];
        }

    }
}
