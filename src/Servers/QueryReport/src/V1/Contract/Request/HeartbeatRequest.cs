using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Exception;

namespace UniSpy.Server.QueryReport.V1.Contract.Request
{
    public class HeartbeatRequest : RequestBase
    {
        public uint QueryReportPort { get; private set; }
        public string GameName { get; private set; }
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
            if (!uint.TryParse(RequestKeyValues["heartbeat"], out var port))
            {
                throw new QRException("Query report port invalid.");
            }
            QueryReportPort = port;
            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new QRException("No gamename found.");
            }
            GameName = RequestKeyValues["gamename"];
        }
    }
}