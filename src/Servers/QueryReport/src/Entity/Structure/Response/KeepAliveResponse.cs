using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Response
{
    public sealed class KeepAliveResponse : ResponseBase
    {
        public KeepAliveResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
