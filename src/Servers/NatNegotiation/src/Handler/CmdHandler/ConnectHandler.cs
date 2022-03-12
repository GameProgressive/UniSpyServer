using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
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
        private List<IClient> _matchedClients;
        private ConnectResponse _responseToServer;
        private ConnectResponse _responseToClient;
        public ConnectHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ConnectResult();
        }
        protected override void RequestCheck()
        {
            _matchedClients = Client.ClientPool.Values.Where(
             c => ((Client)c).Info.Cookie == _request.Cookie).ToList();
            // because cookie is unique for each client we will only get 2 of keys
            if (_matchedClients.Count != 2)
            {
                // throw new NNException("No users match found we continue waitting.");
                LogWriter.Info("No users match found we continue waitting.");
                return;
            }
        }
        protected override void DataOperation()
        {
            if (_matchedClients.Count != 2)
            {
                return;
            }

            //find negitiators and negotiatees by a same cookie
            var client = (Client)_matchedClients.FirstOrDefault(s => ((Client)s).Info.ClientIndex == 0);
            var server = (Client)_matchedClients.FirstOrDefault(s => ((Client)s).Info.ClientIndex == 1);

            if (client is null || server is null)
            {
                LogWriter.ToLog("No match found, we keep waiting!");
                return;
            }
            var waitExpireTime = System.TimeSpan.FromSeconds(5);
            var waitStartTime = System.DateTime.Now;
            // we wait for init process is finished
            while (System.DateTime.Now.Subtract(waitStartTime) < waitExpireTime)
            {
                if (client.Info.IsInitFinished != true
                || server.Info.IsInitFinished != true)
                {
                    LogWriter.ToLog("One of the client is not finish init process, we are waitting...");
                    return;
                }
            }


            // if all client and server is ready we then start try nat punch
            if (client.Info.IsTransitTraffic != true && server.Info.IsTransitTraffic != true)
            {
                AddressCheckHandler.DeterminIPandPortRestriction(client.Info);
                AddressCheckHandler.DeterminIPandPortRestriction(server.Info);
                AddressCheckHandler.DetermineNatPortMapping(client.Info);
                AddressCheckHandler.DetermineNatPortMapping(server.Info);
                AddressCheckHandler.DetermineNatType(client.Info);
                AddressCheckHandler.DetermineNatType(server.Info);
                AddressCheckHandler.DetermineNextAddress(client.Info);
                AddressCheckHandler.DetermineNextAddress(client.Info);
            }
            else
            {
                string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
                var externalIp = IPAddress.Parse(externalIpString);
                // we use natneg server to transit message
                client.Info.GuessedPublicIPEndPoint = new IPEndPoint(externalIp, 27901);
                server.Info.GuessedPublicIPEndPoint = new IPEndPoint(externalIp, 27902);
            }

            var request = new ConnectRequest { Version = _request.Version, Cookie = _request.Cookie };
            _responseToServer = new ConnectResponse(
                request,
                new ConnectResult { RemoteEndPoint = client.Info.GuessedPublicIPEndPoint });

            _responseToClient = new ConnectResponse(
                _request,
                new ConnectResult { RemoteEndPoint = server.Info.GuessedPublicIPEndPoint });

            if (_responseToClient == null || _responseToServer == null)
            {
                return;
            }
            _responseToClient.Build();
            _responseToServer.Build();
            // we send the information to each user
            var session = _client.Session as IUdpSession;
            LogWriter.Info($"Find two users: {client.Session.RemoteIPEndPoint}, {server.Session.RemoteIPEndPoint}, we send connect packet to them.");
            LogWriter.LogNetworkSending(server.Session.RemoteIPEndPoint, _responseToServer.SendingBuffer);
            LogWriter.LogNetworkSending(client.Session.RemoteIPEndPoint, _responseToClient.SendingBuffer);
            server.Session.Send(_responseToServer.SendingBuffer);
            client.Session.Send(_responseToClient.SendingBuffer);

            server.Info.RetryNatNegotiationTime++;
            client.Info.RetryNatNegotiationTime++;
        }
    }
}