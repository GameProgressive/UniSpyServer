
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.UniSpyConfig;

namespace WebServer.Abstraction.BaseClass
{
    internal class WebServerFacotry : UniSpyServerFactory
    {
        public WebServerFacotry()
        {
        }

        protected override void StartServer(UniSpyServerConfig cfg)
        {
            throw new System.NotImplementedException();
        }
    }
}