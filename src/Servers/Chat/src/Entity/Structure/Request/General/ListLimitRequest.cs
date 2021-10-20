using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;

namespace UniSpyServer.Chat.Entity.Structure.Request.General
{
    [RequestContract("LISTLIMIT")]
    public sealed class ListLimitRequest : RequestBase
    {
        public ListLimitRequest(string rawRequest) : base(rawRequest)
        {
        }

        public int MaxNumberOfChannels { get; private set; }
        public string Filter { get; private set; }

        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count != 2)
            {
                throw new Exception.Exception("The number of IRC cmd params in GETKEY request is incorrect.");
            }
            int max;
            if (!int.TryParse(_cmdParams[0], out max))
            {
                throw new Exception.Exception("The max number format is incorrect.");
            }
            MaxNumberOfChannels = max;

            Filter = _cmdParams[1];
        }
    }
}
