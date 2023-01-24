using UniSpyServer.Servers.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Exception;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.Servers.ServerBrowser.Application;

namespace UniSpyServer.Servers.ServerBrowser.V2.Handler.CmdHandler
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

            if (_request.PrefixMessage != null)
            {
                var req = new SendMsgRequest(_request.PrefixMessage);
                req.Parse();
                _client.Info.AdHocMessage = req;
            }

            // when full request is received we publish message to qr
            if (_client.Info.AdHocMessage != null && _request.PostfixMessage != null)
            {
                var gameServer = StorageOperation.Persistance.GetGameServerInfo(_client.Info.AdHocMessage.GameServerPublicIPEndPoint);

                if (gameServer is null)
                {
                    throw new SBException("There is no matching game server regesterd.");
                }

                var message = new ClientMessageRequest()
                {
                    ServerBrowserSenderId = _client.Connection.Server.ServerID,
                    NatNegMessage = _request.PostfixMessage,
                    InstantKey = gameServer.InstantKey,
                    TargetIPEndPoint = gameServer.QueryReportIPEndPoint,
                    CommandName = QueryReport.V2.Entity.Enumerate.RequestType.ClientMessage
                };
                StorageOperation.Persistance.PublishClientMessage(message);
                _client.LogInfo($"Send client message to QueryReport Server: {gameServer.ServerID} [{StringExtensions.ConvertByteToHexString(message.NatNegMessage)}]");
                // set adhoc message to null
                _client.Info.AdHocMessage = null;
            }

        }
    }
}
