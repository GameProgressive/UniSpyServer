using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V2.Entity.Structure.Request;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.ServerBrowser.V2.Entity.Exception;
using UniSpy.Server.QueryReport.V2.Entity.Structure.Request;
using UniSpy.Server.Core.Extensions;
using UniSpy.Server.ServerBrowser.Application;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.ServerBrowser.V2.Handler.CmdHandler
{
    /// <summary>
    /// Natneg message maybe incompelete
    /// when debugging sdk the natneg message will split to 2 request
    /// we have to save first message then wait for next message
    /// </summary>
    public sealed class SendMsgHandler : CmdHandlerBase
    {
        private new SendMsgRequest _request => (SendMsgRequest)base._request;
        public SendMsgHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            var gameServer = StorageOperation.Persistance.GetGameServerInfo(_request.GameServerPublicIPEndPoint);

            if (gameServer is null)
            {
                throw new SBException($"No match server found by address {_request.GameServerPublicIPEndPoint}, we ignore client request.");
            }

            var message = new ClientMessageRequest()
            {
                ServerBrowserSenderId = _client.Connection.Server.ServerID,
                NatNegMessage = _request.ClientMessage,
                InstantKey = gameServer.InstantKey,
                TargetIPEndPoint = gameServer.QueryReportIPEndPoint,
                CommandName = QueryReport.V2.Entity.Enumerate.RequestType.ClientMessage
            };
            StorageOperation.Persistance.PublishClientMessage(message);
            _client.LogInfo($"Send client message to QueryReport Server: {gameServer.ServerID} [{StringExtensions.ConvertByteToHexString(message.NatNegMessage)}]");
        }
    }
}
