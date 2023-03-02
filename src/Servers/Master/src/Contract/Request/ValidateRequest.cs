using UniSpy.Server.Master.Abstraction.BaseClass;

namespace UniSpy.Server.Master.Contract.Request
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
            if (!RequestKeyValues.ContainsKey("validate"))
            {
                throw new Master.Exception("validate request format not correct.");
            }
            ValidateKey = RequestKeyValues["validate"];
        }
    }
}