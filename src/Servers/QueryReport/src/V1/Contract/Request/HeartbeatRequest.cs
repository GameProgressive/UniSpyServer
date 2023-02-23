using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.Exception;

namespace UniSpy.Server.QueryReport.V1.Contract.Request
{
    public class HeartbeatRequest : RequestBase
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
                throw new QRException("No query report port found.");
            }
            if (!int.TryParse(RequestKeyValues["heartbeat"], out var port))
            {
                throw new QRException("Query report port invalid.");
            }
            QueryReportPort = port;
            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new QRException("No gamename found.");
            }
            GameName = RequestKeyValues["gamename"];

            if (RequestKeyValues.ContainsKey("statechanged"))
            {
                IsStateChanged = true;
            }
        }
    }
}