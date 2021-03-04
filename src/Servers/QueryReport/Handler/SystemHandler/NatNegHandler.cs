using Newtonsoft.Json;
using QueryReport.Application;
using QueryReport.Entity.Structure.NatNeg;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Result;
using QueryReport.Network;
using Serilog.Events;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.Network;

namespace QueryReport.Handler.SystemHandler.NatNegCookieManage
{
    internal static class NatNegHandler
    {
        static NatNegHandler()
        {
            var subscriber = UniSpyServerFactoryBase.Redis.GetSubscriber();

            subscriber.Subscribe(
                "NatNegCookieChannel", (channel, message)
                =>
                {
                    NatNegCookie cookie = JsonConvert.DeserializeObject<NatNegCookie>(message);
                    SendNatNegCookieToGameServer(cookie);
                });
        }
        public static void SendNatNegCookieToGameServer(NatNegCookie cookie)
        {
            IPAddress address = IPAddress.Parse(cookie.GameServerRemoteIP);
            int port = int.Parse(cookie.GameServerRemotePort);
            var endPoint = new IPEndPoint(address, port);

            IUniSpySession session;
            QRServerFactory.Server.SessionManager.Sessions.TryGetValue(endPoint,out session);
            if (session == null)
            {
                LogWriter.ToLog(LogEventLevel.Error, "Can not find game server in QR");
                return;
            }

            var result = new ClientMessageResult
            {
                NatNegMessage = cookie.NatNegMessage,
                MessageKey = 0,
                InstantKey = ((QRSession)session).InstantKey
            };

            var response = new ClientMessageResponse(null, result);
            response.Build();
            QRServerFactory.Server.SendAsync(endPoint, response.SendingBuffer);
        }
    }
}
