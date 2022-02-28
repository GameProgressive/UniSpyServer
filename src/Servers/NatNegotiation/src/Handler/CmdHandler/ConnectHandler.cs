using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

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
        public ConnectHandler(IClient client, IRequest request) : base(client, request)
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
                // throw new NNException("No users match found we continue waitting.");
                LogWriter.Info("No users match found we continue waitting.");
            }
        }
        protected override void DataOperation()
        {
            if (_matchedUsers.Count != 2)
            {
                return;
            }
            foreach (var key in _matchedUsers)
            {
                //find negitiators and negotiatees by a same cookie
                var negotiators = _matchedUsers.Where(s => s.ClientIndex == 0);
                var negotiatees = _matchedUsers.Where(s => s.ClientIndex == 1);

                if (negotiators.Count() != 1 || negotiatees.Count() != 1)
                {
                    LogWriter.ToLog("No match found, we keep waiting!");
                    return;
                }

                // we only can find one pair of the users
                _negotiator = negotiators.First();
                _negotiatee = negotiatees.First();

                var request = new ConnectRequest { Version = _request.Version, Cookie = _request.Cookie };
                _responseToNegotiator = new ConnectResponse(
                    request,
                    new ConnectResult { RemoteEndPoint = _negotiatee.RemoteIPEndPoint });

                _responseToNegotiatee = new ConnectResponse(
                    _request,
                    new ConnectResult { RemoteEndPoint = _negotiator.RemoteIPEndPoint });
            }


            if (_responseToNegotiatee == null || _responseToNegotiator == null)
            {
                return;
            }
            _responseToNegotiatee.Build();
            _responseToNegotiator.Build();
            // we send the information to each user
            var session = _client.Session as IUdpSession;
            session.Send(_negotiator.RemoteIPEndPoint, _responseToNegotiator.SendingBuffer);
            session.Send(_negotiatee.RemoteIPEndPoint, _responseToNegotiatee.SendingBuffer);
            // test whether this way can notify users
            var udpClient = new UdpClient();
            LogWriter.Info($"Find two users: {_negotiator.RemoteIPEndPoint}, {_negotiatee.RemoteIPEndPoint}, we send connect packet to them.");
            LogWriter.LogNetworkSending(_negotiator.RemoteIPEndPoint, _responseToNegotiator.SendingBuffer);
            LogWriter.LogNetworkSending(_negotiatee.RemoteIPEndPoint, _responseToNegotiatee.SendingBuffer);
            udpClient.SendAsync(_responseToNegotiator.SendingBuffer, _responseToNegotiator.SendingBuffer.Length, _negotiator.RemoteIPEndPoint);
            udpClient.SendAsync(_responseToNegotiatee.SendingBuffer, _responseToNegotiatee.SendingBuffer.Length, _negotiatee.RemoteIPEndPoint);
        }
    }
}