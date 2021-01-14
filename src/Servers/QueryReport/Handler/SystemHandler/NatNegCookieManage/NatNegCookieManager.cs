using Newtonsoft.Json;
using QueryReport.Application;
using QueryReport.Entity.Structure.NatNeg;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Result;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Handler.SystemHandler.NatNegCookieManage
{
    public class NatNegCookieManager
    {
        public NatNegCookieManager()
        {
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

            var result = new ClientMessageResult();
            result.NatNegMessage = cookie.NatNegMessage;
            result.MessageKey = 0;
            var response = new ClientMessageResponse(null, result);
            response.Build();
            //TODO
            //byte[] clientMsg = new ClientMessageResponse(
            //    cookie.NatNegMessage, MessageKey++, session.InstantKey).BuildResponse();

            //session.SendAsync(clientMsg);
            QRServerManager.Server.SendAsync(endPoint, response.SendingBuffer);

        }
    }
}
