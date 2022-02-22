using System.Xml.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.WebServer.Handler
{
    public class CmdSwitcher : CmdSwitcherBase
    {
        private new string _rawRequest => UniSpyEncoding.GetString((byte[])base._rawRequest);
        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            dynamic xelements = XElement.Parse(_rawRequest);
            var name = xelements.FirstNode.FirstNode.Name.LocalName;
            DeserializeRequest(name, _rawRequest);
        }
    }
}