using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
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
        /// <summary>
        /// Indicate the init is already finished
        /// |cooie|isFinished|
        /// </summary>
        public static Dictionary<uint, bool> ConnectStatus;
        private new ConnectRequest _request => (ConnectRequest)base._request;
        private new ConnectResult _result { get => (ConnectResult)base._result; set => base._result = value; }
        private List<NatInitInfo> _matchedInfos;
        private ConnectResponse _responseToClient;
        private ConnectResponse _responseToServer;
        private Client _gameClient;
        private Client _gameServer;
        static ConnectHandler()
        {
            ConnectStatus = new Dictionary<uint, bool>();
        }
        public ConnectHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ConnectResult();
        }
        protected override void RequestCheck()
        {
            var waitExpireTime = TimeSpan.FromSeconds(5);
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
                    break;
                }
            }

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
            

            var matchedUsers = Client.ClientPool.Values.Where(k => ((Client)k).Info.Cookie == _request.Cookie).ToList();
            // assume the all init result is received, the both client must be in our ClientPool
            _gameClient = (Client)Client.ClientPool.Values.First(k => ((Client)k).Info.Cookie == _client.Info.Cookie && ((Client)k).Info.ClientIndex == NatClientIndex.GameClient);
            _gameServer = (Client)Client.ClientPool.Values.First(k => ((Client)k).Info.Cookie == _client.Info.Cookie && ((Client)k).Info.ClientIndex == NatClientIndex.GameServer);
            if (_gameServer == null || _gameClient == null)
            {
                throw new NNException("Init is finished, but two clients are not found in the ClientPool");
            }


            var clientInfos = _matchedInfos.Where(k => k.ClientIndex == NatClientIndex.GameClient).ToDictionary(k => (NatPortType)k.PortType, k => k);
            var serverInfos = _matchedInfos.Where(k => k.ClientIndex == NatClientIndex.GameServer).ToDictionary(k => (NatPortType)k.PortType, k => k);
            var clientNatProperty = AddressCheckHandler.DetermineNatProperties(clientInfos);
            var serverNatProperty = AddressCheckHandler.DetermineNatProperties(serverInfos);
            var guessedClientIPEndpoint = AddressCheckHandler.GuessTargetAddress(clientNatProperty, clientInfos);
            var guessedServerIPEndpoint = AddressCheckHandler.GuessTargetAddress(serverNatProperty, serverInfos);


            var request = new ConnectRequest { Version = _request.Version, Cookie = _request.Cookie };
            _responseToClient = new ConnectResponse(
                request,
                new ConnectResult { RemoteEndPoint = guessedServerIPEndpoint });

            _responseToServer = new ConnectResponse(
                _request,
                new ConnectResult { RemoteEndPoint = guessedClientIPEndpoint });

            _gameClient.Send(_responseToClient);
            _gameServer.Send(_responseToServer);
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