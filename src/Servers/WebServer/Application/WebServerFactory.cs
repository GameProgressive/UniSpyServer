
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.UniSpyConfig;

namespace WebServer.Abstraction.BaseClass
{
    internal class WebServerFacotry : UniSpyServerFactoryBase
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