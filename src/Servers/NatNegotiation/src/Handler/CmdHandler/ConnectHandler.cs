using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
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
        private List<NatInitInfo> _matchedInfos;
        private ConnectResponse _responseToNegotiator;
        private ConnectResponse _responseToNegotiatee;
        private NatInitInfo _negotiator;
        private NatInitInfo _negotiatee;
        public ConnectHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ConnectResult();
        }
        protected override void RequestCheck()
        {


            var waitExpireTime = TimeSpan.FromSeconds(10);
            var startTime = DateTime.Now;
            // we wait for 10 mins to wait for the other client to init finish
            while (DateTime.Now.Subtract(startTime) < waitExpireTime)
            {
                var recordsCount = _redisClient.Values.Count(k =>
                        k.Cookie == _request.Cookie
                        && k.Version == _request.Version);
                if (recordsCount != 8)
                {
                    // wait for the other side to init
                    Thread.Sleep(1000);
                    continue;
                }
                else
                {
                    _matchedInfos = _redisClient.Values.Where(k =>
                                                                k.Cookie == _request.Cookie
                                                                && k.Version == _request.Version).ToList();
                }
            }
            // if we can not find the matched user, we can not do the negotiation

            // because cookie is unique for each client we will only get 2 of keys
            if (_matchedInfos == null)
            {
                // throw new NNException("No users match found we continue waitting.");
                LogWriter.Info("No users match found we continue waitting.");
            }
        }
        protected override void DataOperation()
        {
            if (_matchedInfos == null)
            {
                return;
            }
            if (_matchedInfos.Count != 8)
            {
                return;
            }
            



            // foreach (var key in _matchedUsers)
            // {
            //     //find negitiators and negotiatees by a same cookie
            //     var negotiators = _matchedUsers.Where(s => s.ClientIndex == NatClientIndex.GameClient);
            //     var negotiatees = _matchedUsers.Where(s => s.ClientIndex == NatClientIndex.GameServer);

            //     if (negotiators.Count() != 1 || negotiatees.Count() != 1)
            //     {
            //         LogWriter.ToLog("No match found, we keep waiting!");
            //         return;
            //     }

            //     // we only can find one pair of the users
            //     _negotiator = negotiators.First();
            //     _negotiatee = negotiatees.First();

            //     var request = new ConnectRequest { Version = _request.Version, Cookie = _request.Cookie };
            //     _responseToNegotiator = new ConnectResponse(
            //         request,
            //         new ConnectResult { RemoteEndPoint = _negotiatee.PublicIPEndPoint });

            //     _responseToNegotiatee = new ConnectResponse(
            //         _request,
            //         new ConnectResult { RemoteEndPoint = _negotiator.PublicIPEndPoint });
            // }


            // if (_responseToNegotiatee == null || _responseToNegotiator == null)
            // {
            //     return;
            // }
            // _responseToNegotiatee.Build();
            // _responseToNegotiator.Build();
            // // we send the information to each user
            // var session = _client.Session as IUdpSession;
            // session.Send(_negotiator.PublicIPEndPoint, _responseToNegotiator.SendingBuffer);
            // session.Send(_negotiatee.PublicIPEndPoint, _responseToNegotiatee.SendingBuffer);
            // // test whether this way can notify users
            // var udpClient = new UdpClient();
            // LogWriter.Info($"Find two users: {_negotiator.PublicIPEndPoint}, {_negotiatee.PublicIPEndPoint}, we send connect packet to them.");
            // LogWriter.LogNetworkSending(_negotiator.PublicIPEndPoint, _responseToNegotiator.SendingBuffer);
            // LogWriter.LogNetworkSending(_negotiatee.PublicIPEndPoint, _responseToNegotiatee.SendingBuffer);
            // udpClient.SendAsync(_responseToNegotiator.SendingBuffer, _responseToNegotiator.SendingBuffer.Length, _negotiator.PublicIPEndPoint);
            // udpClient.SendAsync(_responseToNegotiatee.SendingBuffer, _responseToNegotiatee.SendingBuffer.Length, _negotiatee.PublicIPEndPoint);
        }

        private void UpdateRetryCount()
        {
            foreach (var info in _matchedInfos)
            {
                info.RetryCount++;
                _redisClient.SetValue(info);
            }
        }
    }
}