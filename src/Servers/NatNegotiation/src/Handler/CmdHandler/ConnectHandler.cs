using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.Connect)]
    public sealed class ConnectHandler : CmdHandlerBase
    {
        private new ConnectRequest _request => (ConnectRequest)base._request;
        private new ConnectResult _result { get => (ConnectResult)base._result; set => base._result = value; }
        private List<UserInfo> _matchedUsers;
        private ConnectResponse _responseToNegotiator;
        private ConnectResponse _responseToNegotiatee;
        private UserInfo _negotiator;
        private UserInfo _negotiatee;
        public ConnectHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ConnectResult();
        }

        protected override void RequestCheck()
        {
            _matchedUsers = _redisClient.Values.Where(
             k => k.PortType == _request.PortType
             & k.Cookie == _request.Cookie).ToList();

            // because cookie is unique for each client we will only get 2 of keys
            if (_matchedUsers.Count != 2)
            {
                throw new NNException("No users match found we continue waitting.");
            }


        }
        protected override void DataOperation()
        {
            foreach (var key in _matchedUsers)
            {
                //find negitiators and negotiatees by a same cookie
                var negotiators = _matchedUsers.Where(s => s.RequestInfo.ClientIndex == 0);
                var negotiatees = _matchedUsers.Where(s => s.RequestInfo.ClientIndex == 1);

                if (negotiators.Count() != 1 || negotiatees.Count() != 1)
                {
                    LogWriter.ToLog("No match found, we keep waiting!");
                    return;
                }

                // we only can find one pair of the users
                _negotiator = negotiators.First();
                _negotiatee = negotiatees.First();

                LogWriter.Info($"Find negotiatee {_negotiatee.RemoteIPEndPoint}");
                LogWriter.Info($"Find negotiator {_negotiator.RemoteIPEndPoint}");

                var request = new ConnectRequest { Version = _request.Version, Cookie = _request.Cookie };
                _responseToNegotiator = new ConnectResponse(
                    request,
                    new ConnectResult { RemoteEndPoint = _negotiatee.RemoteIPEndPoint });

                _responseToNegotiatee = new ConnectResponse(
                    _request,
                    new ConnectResult { RemoteEndPoint = _negotiator.RemoteIPEndPoint });
            }
        }
        protected override void Response()
        {
            if (_responseToNegotiatee == null || _responseToNegotiator == null)
            {
                return;
            }
            _responseToNegotiatee.Build();
            _responseToNegotiator.Build();
            // we send the information to each user
            _session.Server.SendAsync(_negotiator.RemoteIPEndPoint, _responseToNegotiator.SendingBuffer);
            _session.Server.SendAsync(_negotiatee.RemoteIPEndPoint, _responseToNegotiatee.SendingBuffer);
        }

    }
}