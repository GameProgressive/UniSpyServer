using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Response
{
    public sealed class CreateRecordResponse : ResponseBase
    {
        public CreateRecordResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}