using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V2.Contract.Request
{

    public class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}