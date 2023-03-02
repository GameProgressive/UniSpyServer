using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.Exception;
using UniSpy.Server.QueryReport.Contract.Request;
using UniSpy.Server.QueryReport.Handler.CmdHandler;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.QueryReport.Aggregate.Redis
{
    public sealed class RedisChannel : UniSpy.Server.Core.Abstraction.BaseClass.RedisChannelBase<ClientMessageRequest>
    {
        public RedisChannel() : base(RedisChannelName.NatNegCookieChannel)
        {
        }
        public override void ReceivedMessage(ClientMessageRequest message)
        {
            IClient client;

            // LogWriter.LogNetworkReceiving(message.TargetIPEndPoint,  message.NatNegMessage, true);
            if (Client.ClientPool.ContainsKey(message.TargetIPEndPoint))
            {
                client = Client.ClientPool[message.TargetIPEndPoint];
            }
            else
            {
                throw new QRException($"Client:{message.TargetIPEndPoint} not found.");
            }
            client.LogInfo($"Get client message from server browser: {message.ServerBrowserSenderId} [{StringExtensions.ConvertByteToHexString(message.NatNegMessage)}]");
            new ClientMessageHandler(client, message).Handle();
        }
    }
}