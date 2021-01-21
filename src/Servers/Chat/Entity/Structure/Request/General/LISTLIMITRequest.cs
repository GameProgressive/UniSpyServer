using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public class LISTLIMITRequest : ChatRequestBase
    {
        public LISTLIMITRequest(string rawRequest) : base(rawRequest)
        {
        }

        public int MaxNumberOfChannels { get; protected set; }
        public string Filter { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }

            if (_cmdParams.Count != 2)
            {
                ErrorCode = false;
                return;
            }
            int max;
            if (!int.TryParse(_cmdParams[0], out max))
            {
                ErrorCode = false;
                return;
            }
            MaxNumberOfChannels = max;

            Filter = _cmdParams[1];

            ErrorCode = true;
        }
    }
}
