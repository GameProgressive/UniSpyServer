using System.Xml.Serialization;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.WebServer.Abstraction;

namespace UniSpyServer.WebServer.Entity.Structure.Response
{
    public class GetMyRecordResponse : ResponseBase
    {
        public GetMyRecordResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new System.NotImplementedException();
        }
    }
}