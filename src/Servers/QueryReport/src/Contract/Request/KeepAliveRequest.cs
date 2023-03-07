using UniSpy.Server.QueryReport.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.Contract.Request
{

    public class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}