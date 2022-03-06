using System.Linq;
using System.Net.Sockets;
using UniSpyServer.Servers.QueryReport.Entity.Structure.NatNeg;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Response;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Result;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Contract;
using UniSpyServer.Servers.ServerBrowser.Entity.Exception;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// we need forward this to game server
    /// </summary>
    [HandlerContract(ServerBrowser.Entity.Enumerate.RequestType.NatNegRequest)]
    public sealed class NatNegMsgHandler : CmdHandlerBase
    {
        private static UdpClient _udpClient = new UdpClient();
        private new NatNegMsgRequest _request => (NatNegMsgRequest)base._request;
        private NatNegCookie _natNegCookie;
        private GameServerInfo _gameServer;
        public NatNegMsgHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_client.Info.AdHocMessage is null)
            {
                throw new SBException("There are no server messages in _session.ServerMessageList.");
            }

            _gameServer = _gameServerRedisClient.Values.FirstOrDefault(x =>
                x.HostIPAddress == _client.Info.AdHocMessage.TargetIPEndPoint.Address &
                x.QueryReportPort == (ushort)_client.Info.AdHocMessage.TargetIPEndPoint.Port);
            if (_gameServer == null)
            {
                throw new SBException("There is no matching game server regesterd.");
            }
            // reset message to null
            // _session.AdHocMessage = null;
        }

        protected override void DataOperation()
        {
            //TODO check the if the remote endpoint is correct

            // !Fix this
            var result = new ClientMessageResult
            {
                NatNegMessage = _request.RawRequest,
                MessageKey = 0,
            };
            var request = new ClientMessageRequest()
            {
                InstantKey = _gameServer.InstantKey,
                CommandName = QueryReport.Entity.Enumerate.RequestType.ClientMessage
            };
            var response = new ClientMessageResponse(request, result);
            response.Build();
            // we send 3 times to make sure the message is received
            LogWriter.LogNetworkSending(_gameServer.QueryReportIPEndPoint, response.SendingBuffer);
            // for (var i = 0; i < 3; i++)
            // {
            _udpClient.Send(response.SendingBuffer,
                                response.SendingBuffer.Length,
                                _gameServer.QueryReportIPEndPoint);
            // }
        }
    }
}
