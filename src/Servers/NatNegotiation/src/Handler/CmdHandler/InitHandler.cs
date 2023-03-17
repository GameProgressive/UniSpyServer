using System.Threading.Tasks;
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Application;
using UniSpy.Server.NatNegotiation.Enumerate;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Response;
using UniSpy.Server.NatNegotiation.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.NatNegotiation.Handler.CmdHandler
{

    public sealed class InitHandler : CmdHandlerBase
    {
        public static readonly int InitPacketCount = 7;
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result { get => (InitResult)base._result; set => base._result = value; }
        /// <summary>
        /// Local NatInitInfo storage, after all init packets are received we send all into redis database
        /// </summary>
        private NatAddressInfo _addressInfo;
        public InitHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new InitResult();
        }
        protected override void DataOperation()
        {
            _addressInfo = new NatAddressInfo()
            {
                ServerID = _client.Connection.Server.ServerID,
                Cookie = (uint)_request.Cookie,
                UseGamePort = _request.UseGamePort,
                ClientIndex = (NatClientIndex)_request.ClientIndex,
                Version = _request.Version,
                PortType = _request.PortType,
                PublicIPEndPoint = _client.Connection.RemoteIPEndPoint,
                PrivateIPEndPoint = _request.PrivateIPEndPoint
            };
            _client.LogInfo($"Received init request with private ip: [{_addressInfo.PrivateIPEndPoint}], cookie: {_addressInfo.Cookie}, client index: {_addressInfo.ClientIndex}.");
            _result.RemoteIPEndPoint = _client.Connection.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
        protected override void Response()
        {
            base.Response();

            lock (_client.Info)
            {
                if (_client.Info.ClientIndex is null)
                {
                    _client.Info.ClientIndex = _request.ClientIndex;
                }
                if (_client.Info.Cookie is null)
                {
                    _client.Info.Cookie = _request.Cookie;
                }
            }

            // todo make the code do not block and redis do not have thread theaf problem
            // Task.Run(() => StorageOperation.Persistance.UpdateInitInfo(_addressInfo));
            StorageOperation.Persistance.UpdateInitInfo(_addressInfo);
            // init packet nn3 is the last one client send, although receiving nn3 does not mean we received other init packets, but we can use this as a flag to prevent start multiple connect handler
            if (_request.PortType == NatPortType.NN3 && _client.Info.IsNeigotiating == false)
            {
                _client.Info.IsNeigotiating = true;
                PrepareForConnectingAsync();
            }
        }


        /// <summary>
        /// Prepare to send connect response
        /// </summary>
        private async void PrepareForConnectingAsync()
        {
            _client.LogInfo($"Watting for negotiator's initInfo with cookie:{_request.Cookie}.");
            await Task.Delay(2000);
            int waitCount = 1;
            // we only wait 8 seconds
            while (waitCount <= 4)
            {
                var initCount = StorageOperation.Persistance.CountInitInfo(_request.Cookie, _request.Version);
                // some client do not send GP init packet, we can only receive 7 packets
                if (initCount >= InitPacketCount)
                {
                    _client.LogInfo("2 neigotiators found, start negotiating.");
                    StartConnecting();
                    break;
                }
                else
                {
                    _client.LogInfo($"No negotiator found with cookie: {_request.Cookie}, retry count: {waitCount}.");
                }
                waitCount++;
                await Task.Delay(2000);
            }
            // if server can not find the client2 within 8 retry, then we log the error. 
            if (waitCount > 4)
            {
                _client.LogWarn($"cookie: {_request.Cookie} have no negotiator found , we clean init information, please connect again.");
            }
        }

        /// <summary>
        ///  Start connect handler to tell each other's public ip and port
        /// </summary>
        private void StartConnecting()
        {
            var request = new ConnectRequest
            {
                Version = _request.Version,
                Cookie = _request.Cookie,
                ClientIndex = _request.ClientIndex
            };
            new ConnectHandler(_client, request).Handle();
        }
    }
}
