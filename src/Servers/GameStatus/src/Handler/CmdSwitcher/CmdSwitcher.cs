﻿using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdSwitcher
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new string _rawRequest => (string)base._rawRequest;

        public CmdSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            if (_rawRequest[0] != '\\')
            {
                throw new UniSpyException("Invalid request");
            }
            string[] splitedRawRequests = _rawRequest.TrimStart('\\').Split('\\');
            foreach (var rawRequest in splitedRawRequests)
            {
                var name = GameSpyUtils.ConvertToKeyValue(rawRequest).Keys.First();
                DeserializeRequest(name, rawRequest);
            }
        }
    }
}
