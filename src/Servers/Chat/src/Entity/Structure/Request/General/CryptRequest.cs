using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Exception;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("CRYPT")]
    public sealed class CryptRequest : RequestBase
    {
        public CryptRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string VersionID { get; private set; }
        public string GameName { get; private set; }
        //CRYPT des %d %s
        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count < 3)
                throw new Exception.Exception("The number of IRC params in CRYPT request is incorrect.");

            VersionID = _cmdParams[1];
            GameName = _cmdParams[2];
        }
    }
}
