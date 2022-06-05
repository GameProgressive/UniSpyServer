using System.Linq;
using UniSpyServer.Servers.GameTrafficRelay.Abstraction.BaseClass;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Contract;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameTrafficRelay.Handler.CmdHandler
{
    [HandlerContract(RequestType.Ping)]
    public sealed class PingHandler : CmdHandlerBase
    {
        private new Client _client => (Client)base._client;
        private new PingRequest _request => (PingRequest)base._request;
        /// <summary>
        /// The first ping packet will process by natneg server,
        /// when the info is saved, next ping will directly send to another client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public PingHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            lock (_client.Info)
            {
                if (_client.Info.PingData is null)
                {
                    _client.Info.PingData = _request.RawRequest;
                    _client.Info.Cookie = _request.Cookie;
                    _client.Info.ClientIndex = _request.ClientIndex;
                }
                else
                {
                    if (!_client.Info.PingData.SequenceEqual(_request.RawRequest))
                    {
                        _client.Info.PingData = _request.RawRequest;
                        _client.Info.Cookie = _request.Cookie;
                        _client.Info.ClientIndex = _request.ClientIndex;
                        // reset the target client
                        _client.Info.TrafficRelayTarget = null;
                    }
                }
                var targetClient = (Client)Client.ClientPool.Values.FirstOrDefault(
                                u => ((Client)u).Info.Cookie == _request.Cookie
                                && ((Client)u).Info.ClientIndex == _request.ClientIndex
                                && u.Session.RemoteIPEndPoint != _client.Session.RemoteIPEndPoint);

                if (targetClient is null)
                {
                    return;
                }
                if (_client.Info.TrafficRelayTarget is null)
                {
                    _client.Info.TrafficRelayTarget = targetClient;
                }
            }
        }
    }
}