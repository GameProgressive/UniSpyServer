using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.Exception;

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
                throw new QRException("No query report port found.");
            }
            if (!int.TryParse(KeyValues["heartbeat"], out var port))
            {
                throw new QRException("Query report port invalid.");
            }
            QueryReportPort = port;
            if (!KeyValues.ContainsKey("gamename"))
            {
                throw new QRException("No gamename found.");
            }
            GameName = KeyValues["gamename"];

            if (KeyValues.ContainsKey("statechanged"))
            {
                IsStateChanged = true;
            }
        }
    }
}