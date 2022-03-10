using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Entity.Constant;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Handler
{
    public class CmdSwitcher : CmdSwitcherBase<RequestContract, HandlerContract>
    {
        private static Dictionary<string, string> _oldRequestMapping = new Dictionary<string, string>{
            { "LoginRemoteAuth", "LoginRemoteAuthWithGameId" },
            { "LoginPs3Cert", "LoginPs3CertWithGameId" },
            { "LoginUniqueNick", "LoginUniqueNickWithGameId" },
            {"LoginProfile", "LoginProfileWithGameId"}
        };

        private new NetCoreServer.HttpRequest _rawRequest => (NetCoreServer.HttpRequest)base._rawRequest;
        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            // var uri = new Uri(_rawRequest.Url);
            // if (WebEndpoints.AvailableEndpoints.Contains(uri.LocalPath))
            // {
            //     throw new UniSpyException($"Invalid http path access:{_rawRequest.Url}");
            // }
            dynamic xelements = XElement.Parse(_rawRequest.Body);
            var name = xelements.FirstNode.FirstNode.Name.LocalName;
            // convert old api to new api name
            if (_oldRequestMapping.ContainsKey(name))
            {
                name = _oldRequestMapping[name];
            }
            DeserializeRequest(name, _rawRequest.Body);
        }
    }
}