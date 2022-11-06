using UniSpyServer.Servers.QueryReport.Entity.Exception;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.Handler.CmdHandler;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Entity.Structure;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Redis
{
    public sealed class RedisChannel : UniSpyLib.Abstraction.BaseClass.Redis.RedisChannel<ClientMessageRequest>
    {
        public RedisChannel() : base(UniSpyRedisChannelName.NatNegCookieChannel)
        {
        }
        public override void ReceivedMessage(ClientMessageRequest message)
        {
            IClient client;
            LogWriter.Info($"Get client message from server browser: {message.ServerBrowserSenderId} [{StringExtensions.ConvertByteToHexString(message.NatNegMessage)}]");
            // LogWriter.LogNetworkReceiving(message.TargetIPEndPoint,  message.NatNegMessage, true);
            if (Client.ClientPool.ContainsKey(message.TargetIPEndPoint))
            {
                client = Client.ClientPool[message.TargetIPEndPoint];
            }
            else
            {
                throw new QRException($"Client:{message.TargetIPEndPoint} not found.");
            }
            new ClientMessageHandler(client, message).Handle();
        }
    }
}