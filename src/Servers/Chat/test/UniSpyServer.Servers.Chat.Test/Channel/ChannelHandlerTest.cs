using System;
using System.Text;
using System.Threading;
using NetCoreServer;
using UniSpyServer.Servers.Chat.Application;
using UniSpyServer.Servers.Chat.Test.Channel;
using UniSpyServer.Servers.Chat.Test.General;
using Xunit;

namespace UniSpyServer.Servers.Chat.Test
{
    public class TestTcpClient : TcpClient
    {
        public bool IsWaitting { get; private set; }
        public string CurrentMessage { get; private set; }
        public TestTcpClient(string ip, int port) : base(ip, port)
        {
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            IsWaitting = false;
            CurrentMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
        }
        public override long Send(string text)
        {
            IsWaitting = true;
            return base.Send(text);
        }
    }
    public class ChannelHandlerTest
    {
        public ChannelHandlerTest()
        {
        }
        [Fact]
        public void JoinTest()
        {
            var client1 = new TestTcpClient("127.0.0.1", 6667);
            client1.Connect();
            client1.ReceiveAsync();
            client1.Send(GeneralRequests.User);
            Thread.Sleep(10000);
            client1.Send(GeneralRequests.Nick);
            Thread.Sleep(10000);
            client1.Send(ChannelRequests.Join);
            Thread.Sleep(10000);
            // Assert.Equal(1, ServerFactory.Server.SessionManager.SessionPool.Count);
        }
    }
}