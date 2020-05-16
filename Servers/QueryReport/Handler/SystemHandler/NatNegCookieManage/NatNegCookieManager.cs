using System.Net;
using GameSpyLib.Common;
using GameSpyLib.Logging;
using Newtonsoft.Json;
using QueryReport.Entity.Structure.NatNeg;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Handler.SystemHandler.QRSessionManage;
using QueryReport.Server;
using StackExchange.Redis;

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
            SubScribe();
        }

        public void SubScribe()
        {
            ISubscriber subscriber = ServerManagerBase.Redis.GetSubscriber();

            subscriber.Subscribe(
                "NatNegCookieChannel", (channel, message)
                =>
                {
                    SendNatNegCookieToGameServer(message);
                });
        }
        public void SendNatNegCookieToGameServer(string data)
        {
            LogWriter.LogCurrentClass(this);
            NatNegCookie cookie = JsonConvert.DeserializeObject<NatNegCookie>(data);
            IPAddress address = IPAddress.Parse(cookie.GameServerRemoteIP);
            int port = int.Parse(cookie.GameServerRemotePort);
            IPEndPoint ipEnd = new IPEndPoint(address, port);
            EndPoint endPoint = ipEnd;
            QRSession session;

            if (!QRSessionManager.Sessions.TryGetValue(endPoint, out session))
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, "Can not find game server in QR");
                return;
            }
            
            byte[] clientMessage = new ClientMessagePacket()
                         .SetMessage(cookie.NatNegMessage)
                         .SetMessageKey(MessageKey++)
                         .SetInstanceKey(session.InstantKey)
                         .BuildResponse();

            session.SendAsync(clientMessage);
        }
    }
}
