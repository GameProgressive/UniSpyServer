using System.Collections.Generic;
using System.Net;
using Moq;
using UniSpyServer.Servers.Chat.Entity.Structure;
using UniSpyServer.Servers.Chat.Handler;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using Xunit;

namespace UniSpyServer.Servers.Chat.Test
{
    public class GameTest
    {
        private Client _client;
        public GameTest()
        {
            var serverMock = new Mock<IServer>();
            serverMock.Setup(s => s.ServerID).Returns(new System.Guid());
            serverMock.Setup(s => s.ServerName).Returns("Chat");
            serverMock.Setup(s => s.Endpoint).Returns(new IPEndPoint(IPAddress.Any, 6666));
            var sessionMock = new Mock<ITcpSession>();
            sessionMock.Setup(s => s.RemoteIPEndPoint).Returns(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));
            sessionMock.Setup(s => s.Server).Returns(serverMock.Object);
            _client = new Client(sessionMock.Object);
        }

        [Fact]
        public void Civilization4()
        {
            var rawRequests = new List<string>(){
                "CRYPT des 1 anno1701",
                "USRIP",
                "USER X419pGl4sX|18 127.0.0.1 peerchat.gamespy.com :aa3041ada9385b28fc4d4e47db288769",
                "NICK a1701-5",
                "CDKEY 81123-67814-77652-27631-11723-47707-22638-10701",
                "JOIN #GSP!anno1701 ","MODE #GSP!anno1701",
                @"GETCKEY #GSP!anno1701 * 008 0 :\b_flags","WHO a1701-5",
                "JOIN #GSP!anno1701!M9zK0KJaKM ",
                "MODE #GSP!anno1701!M9zK0KJaKM",
                @"SETCKEY #GSP!anno1701 a1701-5 :\b_flags\s",
                @"SETCKEY #GSP!anno1701!M9zK0KJaKM a1701-5 :\b_flags\sh",
                @"GETCKEY #GSP!anno1701!M9zK0KJaKM * 009 0 :\b_flags",
                "TOPIC #GSP!anno1701!M9zK0KJaKM :test",
                "MODE #GSP!anno1701!M9zK0KJaKM +l 4",
                "MODE #GSP!anno1701!M9zK0KJaKM -i-p-s+m+n+t+l+e 4",
                "PART #GSP!anno1701 :"
            };

            foreach (var raw in rawRequests)
            {
                new CmdSwitcher(_client, UniSpyEncoding.GetBytes(raw)).Switch();
            }
        }
    }
}