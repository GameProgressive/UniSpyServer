// using System;
// using System.Net;
// using UniSpy.Server.Core.Encryption;
// using UniSpy.Server.Core.Logging;
// using TcpClient = NetCoreServer.TcpClient;

// namespace UniSpy.Server.Core.Network.Tcp.Client
// {

//     public abstract class UniSpyTcpClient : TcpClient
//     {
//         protected UniSpyTcpClient(IPEndPoint endpoint) : base(endpoint)
//         {
//         }

//         /// <summary>
//         /// We automatic connect to remote server address
//         /// </summary>

//         protected override void OnReceived(byte[] buffer, long offset, long size)
//         {
//             LogWriter.LogNetworkTraffic("Recv", Endpoint, buffer, size);
//             byte[] tempBuffer = new byte[size];
//             Array.Copy(buffer, 0, tempBuffer, 0, size);
//             OnReceived(tempBuffer);
//         }

//         protected virtual void OnReceived(byte[] buffer)
//         {
//             OnReceived(UniSpyEncoding.GetString(buffer));
//         }

//         protected virtual void OnReceived(string buffer) { }

//         public override bool SendAsync(byte[] buffer, long offset, long size)
//         {
//             LogWriter.LogNetworkTraffic("Send", Endpoint, buffer, size);
//             return base.SendAsync(buffer, offset, size);
//         }

//         public override long Send(byte[] buffer, long offset, long size)
//         {
//             LogWriter.LogNetworkTraffic("Send", Endpoint, buffer, size);
//             return base.Send(buffer, offset, size);
//         }
//     }
// }
