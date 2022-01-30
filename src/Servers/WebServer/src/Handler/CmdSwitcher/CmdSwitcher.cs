using System.Xml.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Handler
{
    public class CmdSwitcher : UniSpyCmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        public CmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
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