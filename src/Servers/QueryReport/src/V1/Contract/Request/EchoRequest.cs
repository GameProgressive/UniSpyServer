namespace UniSpy.Server.QueryReport.V1.Contract.Request
{
    public class EchoRequest : V1.Abstraction.BaseClass.RequestBase
    {
        public EchoRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            throw new System.NotImplementedException();
        }
    }
}