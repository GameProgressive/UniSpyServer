using System.Xml.Serialization;
using UniSpyLib.Abstraction.BaseClass;
using WebServer.Abstraction;

namespace WebServer.Entity.Structure.Response
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