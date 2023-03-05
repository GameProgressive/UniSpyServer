using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.Exception;
using UniSpy.Server.QueryReport.Contract.Request;
using UniSpy.Server.QueryReport.Handler.CmdHandler;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Abstraction.BaseClass;
using System.Threading.Tasks;

namespace UniSpy.Server.QueryReport.Aggregate.Redis
{
    public class HeartbeatChannel : RedisChannelBase<HeartBeatRequest>
    {
        public HeartbeatChannel() : base(RedisChannelName.HeartbeatChannel)
        {
        }
    }
    public sealed class NatNegChannel : RedisChannelBase<ClientMessageRequest>
    {
        public NatNegChannel() : base(RedisChannelName.NatNegCookieChannel)
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