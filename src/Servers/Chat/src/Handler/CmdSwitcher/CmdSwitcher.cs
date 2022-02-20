using System;
using System.Linq;
using UniSpyServer.Servers.Chat.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CommandSwitcher
{
    /// <summary>
    /// Process request to Commands
    /// </summary>
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            string[] splitedRawRequests = _rawRequest.Replace("\r", "")
                   .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawRequest in splitedRawRequests)
            {
                var name = rawRequest.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().First();
                DeserializeRequest(name, rawRequest);
            }
        }
    }
}
