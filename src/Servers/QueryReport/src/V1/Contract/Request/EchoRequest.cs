using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V1.Contract.Request
{
    public sealed class EchoRequest : RequestBase
    {
        public string Validate { get; private set; }
        public string GameName { get; private set; }
        public EchoRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            if (!KeyValues.ContainsKey("validate"))
            {
                throw new QueryReport.Exception("validate missing from request.");
            }
            if (!KeyValues.ContainsKey("gamename"))
            {
                throw new QueryReport.Exception("gamename missing from request.");
            }

            throw new System.NotImplementedException();
        }
    }
}