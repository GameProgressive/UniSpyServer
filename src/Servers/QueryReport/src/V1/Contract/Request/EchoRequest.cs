using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Exception;

namespace UniSpy.Server.QueryReport.V1.Contract.Request
{
    public class EchoRequest : RequestBase
    {
        public string Validate { get; private set; }
        public string GameName { get; private set; }
        public EchoRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            if (!RequestKeyValues.ContainsKey("validate"))
            {
                throw new QRException("validate missing from request.");
            }
            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new QRException("gamename missing from request.");
            }

            throw new System.NotImplementedException();

        }
    }
}