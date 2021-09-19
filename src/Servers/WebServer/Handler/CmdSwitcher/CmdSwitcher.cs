using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace WebServer.Handler
{
    internal class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _message => (string)base._message;
        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            throw new System.NotImplementedException();
        }
    }
}