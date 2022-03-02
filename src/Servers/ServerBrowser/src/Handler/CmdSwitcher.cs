using UniSpyServer.Servers.ServerBrowser.Entity.Contract;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase<RequestContract, HandlerContract>
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;

        public CmdSwitcher(IClient client, byte[] rawRequest) : base(client, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            var name = (RequestType)_rawRequest[2];
            DeserializeRequest(name, _rawRequest);
        }
    }
}
