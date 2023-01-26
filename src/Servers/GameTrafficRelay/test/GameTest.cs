// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using Moq;
// using UniSpyServer.Servers.GameTrafficRelay.Entity;
// using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure;
// using UniSpyServer.Servers.GameTrafficRelay.Handler;
// using UniSpyServer.UniSpyLib.Abstraction.Interface;
// using Xunit;

// namespace UniSpyServer.Servers.GameTrafficRelay.Test
// {
//     public class GameTest
//     {
//         [Fact]
//         public void Anno1701_20230125()
//         {

//             int portStart = 1000;
//             // var (port1, port2) = NetworkUtils.GetAvailablePort(portStart);
//             var dd = IPAddress.IsLoopback(IPAddress.Parse("127.0.0.1"));
//             { }
//             /*
//             [23:30:45] [DBUG] [91.43.55.217:1027] [Recv] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[00][00]
//             [23:30:45] [DBUG] [91.43.55.217:21701] [Recv] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[00][00]
//             [23:30:45] [VERB] [ => ] [PingRequest]
//             [23:30:45] [VERB] [ => ] [PingRequest]
//             [23:30:45] [VERB] [ => ] [PingHandler]
//             [23:30:45] [VERB] [ => ] [PingHandler]
//             [23:30:45] [DBUG] [91.43.55.217:21701] [Recv] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[00][00]
//             [23:30:45] [VERB] [ => ] [PingRequest]
//             [23:30:45] [VERB] [ => ] [PingHandler]
//             [23:30:45] [VERB] [91.43.55.217:1027] => [91.43.55.217:21701] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[00][00]
//             [23:30:46] [DBUG] [91.43.55.217:21701] [Recv] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             [23:30:46] [VERB] [ => ] [PingRequest]
//             [23:30:46] [VERB] [ => ] [PingHandler]
//             [23:30:46] [VERB] [91.43.55.217:1027] => [91.43.55.217:21701] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[00][00]
//             [23:30:46] [VERB] [91.43.55.217:21701] => [91.43.55.217:1027] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             [23:30:46] [DBUG] [91.43.55.217:1027] [Recv] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             [23:30:46] [VERB] [ => ] [PingRequest]
//             [23:30:46] [VERB] [ => ] [PingHandler]
//             [23:30:47] [VERB] [91.43.55.217:21701] => [91.43.55.217:1027] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             [23:30:48] [VERB] [91.43.55.217:21701] => [91.43.55.217:1027] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             [23:30:48] [VERB] [91.43.55.217:21701] => [91.43.55.217:1027] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             [23:30:49] [VERB] [91.43.55.217:21701] => [91.43.55.217:1027] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             [23:30:50] [VERB] [91.43.55.217:21701] => [91.43.55.217:1027] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             [23:30:50] [VERB] [91.43.55.217:21701] => [91.43.55.217:1027] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             [23:30:51] [VERB] [91.43.55.217:21701] => [91.43.55.217:1027] [FD][FC][1E]fj[B2][03][07][00][00]5z[A1]a[8D]Y'f[01][00]
//             */
//             var client = TestClasses.CreateClient("91.43.55.217", 21701);
//             var server = TestClasses.CreateClient("91.43.55.217", 1027);
//             var clientRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
//             var clientRequest2 = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x01, 0x00 };
//             var serverRequest = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
//             // Given
//             var reqs = new List<KeyValuePair<ITestClient, byte[]>>()
//             {
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)server,serverRequest),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)server,serverRequest),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest2),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)server,serverRequest),

//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest2),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest2),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest2),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest2),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest2),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest2),
//                 new KeyValuePair<ITestClient,byte[]>((ITestClient)client,clientRequest2)
//             };

//             // sync test
//             foreach (var item in reqs)
//             {
//                 item.Key.TestReceived(item.Value);
//             }




//         }
//         [Fact]
//         public void PingNormalTest()
//         {

//             var client1 = TestClasses.CreateClient("192.168.1.2", 9999);
//             var client2 = TestClasses.CreateClient("192.168.1.3", 9999);

//             var client1Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
//             var client2Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

//             var requests = new List<KeyValuePair<string, byte[]>>()
//             {
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//             };
//             var clients = new Dictionary<string, IClient>()
//             {
//                 {"client1",client1},
//                 {"client2",client2}
//             };

//             foreach (var req in requests)
//             {
//                 ((ITestClient)clients[req.Key]).TestReceived(req.Value);
//             }

//             // Assert.True(client1.Info.TrafficRelayTarget.Connection.RemoteIPEndPoint.Equals(client2.Connection.RemoteIPEndPoint));
//             // Assert.True(client2.Info.TrafficRelayTarget.Connection.RemoteIPEndPoint.Equals(client1.Connection.RemoteIPEndPoint));
//         }

//         [Fact]
//         /// <summary>
//         /// A new ping packet with same ip and port incoming
//         /// </summary>
//         public void UpdatedPacketPingTest()
//         {
//             var client1 = TestClasses.CreateClient("192.168.1.2", 9999);
//             var client2 = TestClasses.CreateClient("192.168.1.3", 9999);

//             var client1Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
//             var client2Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

//             var requests = new List<KeyValuePair<string, byte[]>>()
//             {
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//             };
//             var clients = new Dictionary<string, IClient>()
//             {
//                 {"client1",client1},
//                 {"client2",client2}
//             };

//             foreach (var req in requests)
//             {
//                 ((ITestClient)clients[req.Key]).TestReceived(req.Value);
//             }

//             // new ping packet with same IP and port
//             client1Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9B, 0xC0, 0xA8, 0x01, 0x67, 0x6C, 0xFD, 0x00, 0x00 };
//             client2Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9B, 0xC0, 0xA8, 0x00, 0x67, 0x6C, 0xFD, 0x00, 0x00 };

//             requests = new List<KeyValuePair<string, byte[]>>()
//             {
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//             };


//             foreach (var req in requests)
//             {
//                 ((ITestClient)clients[req.Key]).TestReceived(req.Value);
//             }

//             // Assert.True(client1.Info.TrafficRelayTarget.Connection.RemoteIPEndPoint == client2.Connection.RemoteIPEndPoint);
//             // Assert.True(client2.Info.TrafficRelayTarget.Connection.RemoteIPEndPoint == client1.Connection.RemoteIPEndPoint);
//         }

//         [Fact]
//         /// <summary>
//         /// New ping packet with changed server ip and port
//         /// </summary>
//         public void JuliusRealTest()
//         {
//             var client1 = TestClasses.CreateClient("79.209.235.252", 52577);
//             var client2 = TestClasses.CreateClient("211.83.127.54", 50816);
//             var client1Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xA1, 0x61, 0x8D, 0x59, 0x27, 0x66, 0x00, 0x00 };
//             var client2Request = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2, 0x03, 0x07, 0x00, 0x00, 0x02, 0x9A, 0xA1, 0x61, 0x8D, 0x59, 0x27, 0x66, 0x00, 0x00 };

//             var requests = new List<KeyValuePair<string, byte[]>>()
//             {
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//                 new KeyValuePair<string,byte[]>("client1",client1Request),
//                 new KeyValuePair<string,byte[]>("client2",client2Request),
//             };

//             var clients = new Dictionary<string, IClient>()
//             {
//                 {"client1",client1},
//                 {"client2",client2}
//             };

//             foreach (var req in requests)
//             {
//                 ((ITestClient)clients[req.Key]).TestReceived(req.Value);
//             }
//             // Assert.True(client1.Info.TrafficRelayTarget.Connection.RemoteIPEndPoint.Equals(client2.Connection.RemoteIPEndPoint));
//             // Assert.True(client2.Info.TrafficRelayTarget.Connection.RemoteIPEndPoint.Equals(client1.Connection.RemoteIPEndPoint));
//         }
//     }
// }