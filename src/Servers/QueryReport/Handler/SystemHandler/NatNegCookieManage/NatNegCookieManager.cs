using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;
using Newtonsoft.Json;
using QueryReport.Entity.Structure.NatNeg;
using QueryReport.Entity.Structure.Request;
using QueryReport.Handler.SystemHandler.QRSessionManage;
using QueryReport.Network;
using Serilog.Events;
using System.Net;

namespace QueryReport.Handler.SystemHandler.NatNegCookieManage
{
    public class NatNegCookieManager
    {
        private int MessageKey;

        public NatNegCookieManager()
        {
            MessageKey = 0;
        }

        public void Start()
        {
            var subscriber = UniSpyServerManagerBase.Redis.GetSubscriber();

            subscriber.Subscribe(
                "NatNegCookieChannel", (channel, message)
                =>
                {
                    SendNatNegCookieToGameServer(message);
                });
        }

        public void SendNatNegCookieToGameServer(string message)
        {
            NatNegCookie cookie = JsonConvert.DeserializeObject<NatNegCookie>(message);
            IPAddress address = IPAddress.Parse(cookie.GameServerRemoteIP);
            int port = int.Parse(cookie.GameServerRemotePort);
            EndPoint endPoint = new IPEndPoint(address, port);
            QRSession session;

            if (!QRSessionManager.Sessions.TryGetValue(endPoint, out session))
            {
                LogWriter.ToLog(LogEventLevel.Error, "Can not find game server in QR");
                return;
            }

            byte[] clientMsg = new ClientMessageResponse(
                cookie.NatNegMessage, MessageKey++, session.InstantKey).BuildResponse();

            session.SendAsync(clientMsg);
        }
    }
}
