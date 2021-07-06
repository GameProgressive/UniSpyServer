using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace WebServer.Handler
{
    internal class WebSwitcher : UniSpyCmdSwitcher
    {
        public WebSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void DeserializeRequests()
        {
            throw new System.NotImplementedException();
        }
    }
}