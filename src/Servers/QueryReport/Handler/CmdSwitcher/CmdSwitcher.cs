using QueryReport.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace QueryReport.Handler.CmdSwitcher
{
    internal sealed class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new byte[] _message => (byte[])base._message;
        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            if (_message.Length < 4)
            {
                throw new UniSpyException("Invalid request");
            }
            var name = (RequestType)_message[0];
            _rawRequests.Add(name, _message);
        }
    }
}
