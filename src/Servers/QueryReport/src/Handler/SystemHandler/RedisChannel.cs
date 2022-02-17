// using System.Net;
// using UniSpyServer.Servers.QueryReport.Entity.Exception;
// using UniSpyServer.Servers.QueryReport.Entity.Structure.NATNeg;
// using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
// using UniSpyServer.Servers.QueryReport.Entity.Structure.Response;
// using UniSpyServer.Servers.QueryReport.Entity.Structure.Result;
// using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis;
// using UniSpyServer.UniSpyLib.Entity.Structure;

// namespace UniSpyServer.Servers.QueryReport.Handler.SystemHandler
// {
//     public class RedisChannel : RedisChannel<NatNegCookie>
//     {
//         public RedisChannel() : base(UniSpyRedisChannelName.NatNegCookieChannel)
//         {
//         }

//         public override void ReceivedMessage(NatNegCookie message)
//         {
//             var serverEndPoint = new IPEndPoint(message.HostIPAddress, message.HostPort);
//             if (!ServerFactory.Server.SessionManager.SessionPool.ContainsKey(message.HeartBeatIPEndPoint))
//             {
//                 throw new QRException("Can not find game server in QR");
//             }
//             var ss = new Session(ServerFactory.Server, serverEndPoint);
//             var session = ServerFactory.Server.SessionManager.SessionPool[message.HeartBeatIPEndPoint];
//             var result = new ClientMessageResult
//             {
//                 NatNegMessage = message.NatNegMessage,
//                 MessageKey = 0,
//             };
//             var request = new ClientMessageRequest()
//             {
//                 InstantKey = ((Session)session).InstantKey
//             };
//             var response = new ClientMessageResponse(request, result);
//             response.Build();
//             session.Send(response);
//             ServerFactory.Server.SendAsync(serverEndPoint, response.SendingBuffer);
//         }
//     }
// }