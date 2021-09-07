using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CommandSwitcher
{
    internal sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new byte[] _message => (byte[])base._message;

        public CmdSwitcher(IUniSpySession session, byte[] rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            var name = (RequestType)_message[2];
            _rawRequests.Add(name, _message);
        }
    }
}
