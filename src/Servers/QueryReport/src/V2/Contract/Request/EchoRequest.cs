using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V2.Contract.Request
{

    public class EchoRequest : RequestBase
    {
        public EchoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}