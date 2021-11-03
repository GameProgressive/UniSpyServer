using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Handler.CmdSwitcher
{
    public sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;
        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            var req = base._rawRequest as byte[];
            if (_rawRequest.Length < 4)
            {
                throw new UniSpyException("Invalid request");
            }
            var name = (RequestType)_rawRequest[0];
            _cmdMapping.Add(name, _rawRequest);
        }
    }
}
