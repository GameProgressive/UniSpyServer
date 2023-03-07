using UniSpy.Server.QueryReport.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.Contract.Request
{

    public class EchoRequest : RequestBase
    {
        public EchoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}