using UniSpy.Server.Master.Abstraction.BaseClass;

namespace UniSpy.Server.Master.Contract.Request
{
    public sealed class HeartbeatRequest : RequestBase
    {
        public int QueryReportPort { get; private set; }
        public string GameName { get; private set; }
        public bool IsStateChanged { get; private set; }
        public HeartbeatRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (!RequestKeyValues.ContainsKey("heartbeat"))
            {
                throw new Master.Exception("No query report port found.");
            }
            if (!int.TryParse(RequestKeyValues["heartbeat"], out var port))
            {
                throw new Master.Exception("Query report port invalid.");
            }
            QueryReportPort = port;
            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new Master.Exception("No gamename found.");
            }
            GameName = RequestKeyValues["gamename"];

            if (RequestKeyValues.ContainsKey("statechanged"))
            {
                IsStateChanged = true;
            }
        }
    }
}