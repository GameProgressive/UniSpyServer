using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Handler
{
    public class CmdSwitcher : CmdSwitcherBase<RequestContract, HandlerContract>
    {
        private new NetCoreServer.HttpRequest _rawRequest => (NetCoreServer.HttpRequest)base._rawRequest;
        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            dynamic xelements = XElement.Parse(_rawRequest.Body);
            var name = xelements.FirstNode.FirstNode.Name.LocalName;
            DeserializeRequest(name, _rawRequest);
        }
    }
}