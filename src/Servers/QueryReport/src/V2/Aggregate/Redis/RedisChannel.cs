using UniSpy.Server.QueryReport.V2.Contract.Request;
using UniSpy.Server.QueryReport.V2.Handler.CmdHandler;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V2.Aggregate.Redis
{
    public sealed class NatNegChannel : RedisChannelBase<ClientMessageRequest>
    {
        public NatNegChannel() : base(RedisChannelName.NatNegCookieChannel)
        {
        }
        public override void ReceivedMessage(ClientMessageRequest message)
        {
            IClient client = ClientManagerBase.GetClient(message.TargetIPEndPoint);
            if (client is null)
            {
                LogWriter.LogWarn($"Client:{message.TargetIPEndPoint} not found, we ignore natneg message from SB: {message.ServerBrowserSenderId}");
                return;
            }

            client.LogInfo($"Get client message from server browser: {message.ServerBrowserSenderId} [{StringExtensions.ConvertByteToHexString(message.NatNegMessage)}]");
            new ClientMessageHandler(client, message).Handle();
        }
    }
}