﻿using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.ErtAck)]
    public sealed class ErtAckHandler : CmdHandlerBase
    {
        private new ErtAckRequest _request => (ErtAckRequest)base._request;
        private new ErtAckResult _result { get => (ErtAckResult)base._result; set => base._result = value; }
        public ErtAckHandler(ISession session, IRequest request) : base(session, request)
        {
            _result = new ErtAckResult();
        }
        protected override void DataOperation()
        {
            _result.RemoteIPEndPoint = _session.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
