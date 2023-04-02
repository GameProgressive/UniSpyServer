
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V1.Contract.Request
{
    public sealed class ValidateRequest : RequestBase
    {
        public string ValidateKey { get; private set; }
        public ValidateRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            if (!KeyValues.ContainsKey("validate"))
            {
                throw new QueryReport.Exception("validate request format not correct.");
            }
            ValidateKey = KeyValues["validate"];
        }
    }
}