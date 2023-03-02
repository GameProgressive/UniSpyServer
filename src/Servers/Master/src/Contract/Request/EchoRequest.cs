using UniSpy.Server.Master.Abstraction.BaseClass;

namespace UniSpy.Server.Master.Contract.Request
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
            if (!RequestKeyValues.ContainsKey("validate"))
            {
                throw new Master.Exception("validate missing from request.");
            }
            if (!RequestKeyValues.ContainsKey("gamename"))
            {
                throw new Master.Exception("gamename missing from request.");
            }

            throw new System.NotImplementedException();
        }
    }
}