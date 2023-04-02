using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Contract.Request
{
    public class ListRequest : RequestBase
    {
        public ListRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            throw new System.NotImplementedException();
        }
    }
}