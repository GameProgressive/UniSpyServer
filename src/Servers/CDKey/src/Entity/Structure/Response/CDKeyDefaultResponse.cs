using UniSpyServer.CDkey.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.CDkey.Entity.Structure.Response
{
    public sealed class CDKeyDefaultResponse : ResponseBase
    {
        public CDKeyDefaultResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new System.NotImplementedException();
        }
    }
}