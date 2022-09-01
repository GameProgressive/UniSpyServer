using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
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
        private Client _gameClient;
        private Client _gameServer;
        private static GameTrafficRelay.Entity.Structure.Redis.RedisClient _relayRedisClient;
        static ConnectHandler()
        {
            _relayRedisClient = new GameTrafficRelay.Entity.Structure.Redis.RedisClient();
        }
        public ConnectHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ConnectResult();
        }
        protected override void RequestCheck()
        {
            // we need to wait for 2 seconds for another packets
            Thread.Sleep(2000);
            // !! the initResult count is valid in InitHandler, we do not need validate it again
            _matchedInfos = _redisClient.Context.Where(k =>
                                        k.Cookie == _request.Cookie
                                     && k.Version == _request.Version).ToList();
            if (_matchedInfos?.Count != 8)
            {
                throw new NNException("No users match found we continue waitting.");
                // LogWriter.Info("No users match found we continue waitting.");
            }
        }
        protected override void DataOperation()
        {

            IPEndPoint guessedClientIPEndPoint, guessedServerIPEndPoint;
            var clientRemoteIPEnd = _matchedInfos.Where(k =>
                k.ServerID == _client.Session.Server.ServerID
                && k.ClientIndex == NatClientIndex.GameClient
                && k.PortType == NatPortType.GP)
                .Select(k => k.PublicIPEndPoint).First();

            var serverRemoteIPEnd = _matchedInfos.Where(k =>
                k.ServerID == _client.Session.Server.ServerID
                && k.ClientIndex == NatClientIndex.GameServer
                && k.PortType == NatPortType.GP)
                .Select(k => k.PublicIPEndPoint).First();

            _gameClient = (Client)Client.ClientPool[clientRemoteIPEnd];
            _gameServer = (Client)Client.ClientPool[serverRemoteIPEnd];
            if (_gameServer is null || _gameClient is null)
            {
                throw new NNException("Init is finished, but two clients are not found in the ClientPool");
            }

            if (_request.IsUsingRelay)
            {
                var relayServers = _relayRedisClient.Context.ToList();
                var relayEndPoint = relayServers.OrderBy(x => x.ClientCount).First().PublicIPEndPoint;
                guessedClientIPEndPoint = relayEndPoint;
                guessedServerIPEndPoint = relayEndPoint;
            }
            else
            {
                var clientInfos = _matchedInfos.Where(k => k.ClientIndex == NatClientIndex.GameClient)
                                .ToDictionary(k => (NatPortType)k.PortType, k => k);
                var serverInfos = _matchedInfos.Where(k => k.ClientIndex == NatClientIndex.GameServer)
                                .ToDictionary(k => (NatPortType)k.PortType, k => k);

                // if two client is in same lan, we send each privateIPEndPoint
                if (AddressCheckHandler.IsInSameLan(clientInfos, serverInfos))
                {
                    guessedClientIPEndPoint = new IPEndPoint(clientInfos[NatPortType.GP].GPPrivateIPEndPoint.Address,
                             clientInfos[NatPortType.NN2].GPPrivateIPEndPoint.Port - 1);
                    guessedServerIPEndPoint = new IPEndPoint(serverInfos[NatPortType.GP].GPPrivateIPEndPoint.Address,
                            serverInfos[NatPortType.NN2].GPPrivateIPEndPoint.Port - 1);
                }
                else
                {
                    var clientNatProperty = AddressCheckHandler.DetermineNatType(clientInfos);
                    var serverNatProperty = AddressCheckHandler.DetermineNatType(serverInfos);
                    guessedClientIPEndPoint = AddressCheckHandler.GuessTargetAddress(clientNatProperty, clientInfos);
                    guessedServerIPEndPoint = AddressCheckHandler.GuessTargetAddress(serverNatProperty, serverInfos);
                }
                LogWriter.Debug($"client real: {clientRemoteIPEnd}, guessed: {guessedClientIPEndPoint}");
                LogWriter.Debug($"server real: {serverRemoteIPEnd}, guessed: {guessedServerIPEndPoint}");
            }

            var responseToClient = new ConnectResponse(
                _request,
                new ConnectResult { RemoteEndPoint = guessedServerIPEndPoint });

            var responseToServer = new ConnectResponse(
                _request,
                new ConnectResult { RemoteEndPoint = guessedClientIPEndPoint });
            if (_request.ClientIndex == NatClientIndex.GameClient)
            {
                _gameClient.Send(responseToClient);
            }
            else
            {
                _gameServer.Send(responseToServer);
            }
        }
    }
}
