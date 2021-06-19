using QueryReport.Application;
using QueryReport.Entity.Exception;
using QueryReport.Entity.Structure.NATNeg;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using QueryReport.Network;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Entity.Structure;

namespace QueryReport.Handler.SystemHandler
{
    public class QRRedisChannelSubscriber : UniSpyRedisChannelSubscriberBase<NATNegCookie>
    {
        public QRRedisChannelSubscriber() : base(UniSpyRedisChannelName.NatNegCookieChannel)
        {
        }

        public override void ReceivedMessage(NATNegCookie message)
        {
            IPAddress address = IPAddress.Parse(message.GameServerRemoteIP);
            int port = int.Parse(message.GameServerRemotePort);
            var endPoint = new IPEndPoint(address, port);

            IUniSpySession session;
            if (!QRServerFactory.Server.SessionManager.SessionPool.TryGetValue(endPoint, out session))
            {
                throw new QRException("Can not find game server in QR");
            }

            var result = new ClientMessageResult
            {
                NatNegMessage = message.NatNegMessage,
                MessageKey = 0,
                InstantKey = ((QRSession)session).InstantKey
            };
            var request = new ClientMessageRequest()
            {
                InstantKey = ((QRSession)session).InstantKey
            };
            var response = new ClientMessageResponse(request, result);
            response.Build();
            QRServerFactory.Server.SendAsync(endPoint, response.SendingBuffer);
        }
    }
}