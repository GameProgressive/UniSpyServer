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
        // public static Dictionary<uint, bool> ConnectStatus;
        private new ConnectRequest _request => (ConnectRequest)base._request;
        private new ConnectResult _result { get => (ConnectResult)base._result; set => base._result = value; }
        private List<NatInitInfo> _matchedInfos;
        private ConnectResponse _responseToClient;
        private ConnectResponse _responseToServer;
        private Client _gameClient;
        private Client _gameServer;
        public ConnectHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ConnectResult();
        }
        protected override void RequestCheck()
        {
            // !! the initResult count is valid in InitHandler, we do not need validate it again
            _matchedInfos = _redisClient.Values.Where(k =>
                                        k.Cookie == _request.Cookie
                                     && k.Version == _request.Version).ToList();
            if (_matchedInfos?.Count != 8)
            {
                // throw new NNException("No users match found we continue waitting.");
                LogWriter.Info("No users match found we continue waitting.");
            }
        }



        protected override void DataOperation()
        {
            var clientRemoteIPEnd = _matchedInfos.Where(k =>
                k.ServerID == _client.Session.Server.ServerID
                && k.ClientIndex == NatClientIndex.GameClient
                && k.PortType == NatPortType.NN3).Select(k => k.PublicIPEndPoint).First();

            var serverRemoteIPEnd = _matchedInfos.Where(k =>
                k.ServerID == _client.Session.Server.ServerID
                && k.ClientIndex == NatClientIndex.GameServer
                && k.PortType == NatPortType.NN3).Select(k => k.PublicIPEndPoint).First();

            _gameClient = (Client)Client.ClientPool[clientRemoteIPEnd];
            _gameServer = (Client)Client.ClientPool[serverRemoteIPEnd];


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
    }
}