using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;


namespace UniSpy.Server.QueryReport.V1.Contract.Request
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
            if (!KeyValues.ContainsKey("heartbeat"))
            {
                throw new QueryReport.Exception("No query report port found.");
            }
            if (!int.TryParse(KeyValues["heartbeat"], out var port))
            {
                throw new QueryReport.Exception("Query report port invalid.");
            }
            QueryReportPort = port;
            if (!KeyValues.ContainsKey("gamename"))
            {
                throw new QueryReport.Exception("No gamename found.");
            }
            GameName = KeyValues["gamename"];

            if (KeyValues.ContainsKey("statechanged"))
            {
                IsStateChanged = true;
            }
        }
    }
}