using QueryReport.Application;
using QueryReport.Entity.Structure.NATNeg;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using QueryReport.Network;
using Serilog.Events;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Entity.Structure;
using UniSpyLib.Logging;

namespace QueryReport.Handler.SystemHandler
{
    internal class QRRedisChannelEventHandler : UniSpyRedisChannelEventBase<NATNegCookie>
    {
        public QRRedisChannelEventHandler()
        {
            _redisChannelName = UniSpyRedisChannelName.NatNegCookieChannel;
        }

        public override void ReceivedMessage(NATNegCookie message)
        {
            IPAddress address = IPAddress.Parse(message.GameServerRemoteIP);
            int port = int.Parse(message.GameServerRemotePort);
            var endPoint = new IPEndPoint(address, port);

            IUniSpySession session;

            if (!QRServerFactory.Server.SessionManager.SessionPool.TryGetValue(endPoint, out session))
            {
                LogWriter.ToLog(LogEventLevel.Error, "Can not find game server in QR");
                return;
            }

            var result = new ClientMessageResult
            {
                NatNegMessage = message.NatNegMessage,
                MessageKey = 0,
                InstantKey = ((QRSession)session).InstantKey
            };

            var response = new ClientMessageResponse(null, result);
            response.Build();
            QRServerFactory.Server.SendAsync(endPoint, response.SendingBuffer);
        }
    }
}