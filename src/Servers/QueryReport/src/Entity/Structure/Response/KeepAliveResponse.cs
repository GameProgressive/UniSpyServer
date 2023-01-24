using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Response
{
    public sealed class KeepAliveResponse : ResponseBase
    {
        public KeepAliveResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
    }
}
