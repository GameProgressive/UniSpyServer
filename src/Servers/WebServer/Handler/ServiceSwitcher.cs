using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace WebServer.Handler
{
    internal class WebSwitcher : UniSpyCmdSwitcherBase
    {
        public WebSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            throw new System.NotImplementedException();
        }
    }
}