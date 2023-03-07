using UniSpy.Server.Chat.Application;
using Xunit;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Test.Channel;
using UniSpy.Server.Chat.Handler.CmdHandler.Channel;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Aggregate.Redis;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Test.General;
using UniSpy.Server.Core.Abstraction.Interface;
using System.Collections.Generic;

namespace UniSpy.Server.Chat.Test
{
    public class RedisChatChannelTest
    {
        [Fact]
        public void JsonSerialization()
        {

            var client = MockObject.CreateClient() as Client;

            client.Info.IsLoggedIn = true;
            client.Info.NickName = "xiaojiuwo1";
            var remoteClient = client.GetRemoteClient();
            var request = new JoinRequest(ChannelRequests.Join);
            request.Parse();
            var clientStr = JsonConvert.SerializeObject(remoteClient);
            var clientObj = JsonConvert.DeserializeObject<RemoteClient>(clientStr);
            var message = new RemoteMessage(request, remoteClient);
            var msgStr = JsonConvert.SerializeObject(message);
            var msgObj = JsonConvert.DeserializeObject<RemoteMessage>(msgStr);
            Assert.True(msgObj.Type == "JOIN");
            // When
            var req = new JoinRequest(UniSpyEncoding.GetString(msgObj.RawRequest));
            var handler = new JoinHandler(msgObj.Client, req);
            handler.Handle();
            Assert.True(((ClientInfo)(msgObj.Client.Info)).JoinedChannels.Count == 1);
            // Then
        }
        [Fact]
        public void RedisChannel()
        {
            var client = MockObject.CreateClient() as Client;

            client.Info.IsLoggedIn = true;
            client.Info.NickName = "xiaojiuwo2";
            var remoteClient = client.GetRemoteClient();
            var request = new JoinRequest(ChannelRequests.Join);
            request.Parse();
            var message = new RemoteMessage(request, remoteClient);
            // Given
            var msgStr = JsonConvert.SerializeObject(message);
            var msgObj = JsonConvert.DeserializeObject<RemoteMessage>(msgStr);
            var chan = new GeneralMessageChannel();
            chan.ReceivedMessage(msgObj);
            // When

            // Then
        }
        [Fact]
        public void Crypt()
        {
            var request1 = new DisconnectRequest();
            var client = MockObject.CreateClient() as Client;

            client.Info.IsLoggedIn = true;
            client.Info.NickName = "xiaojiuwo3";
            var remoteClient = client.GetRemoteClient();
            var request = new CryptRequest(GeneralRequests.Crypt);
            request.Parse();
            var message = new RemoteMessage(request, remoteClient);
            var chan = new GeneralMessageChannel();
            chan.ReceivedMessage(message);
        }
        [Fact]
        public void GeneralTest()
        {
            var type = typeof(GeneralRequests);
            // var fields = type.GetFields();
            var request1 = new List<string>()
            {
                "CRYPT des 1 worms3\r\n",
                "USRIP\r\n",
                "USER X419pGl4sX|7 127.0.0.1 peerchat.gamespy.com :9964fa3fe73f6a1fbb986d2a04b1eb65\r\n",
                "NICK worms10\r\n",
                "JOIN #GPG!622\r\n",
                "MODE #GPG!622\r\n",
                @"GETCKEY #GPG!622 * 000 0 :\username\b_flags"+"\r\n",
                "PRIVMSG #GPG!622 :hello\r\n",
                "JOIN #GSP!worms3!MJ0NJ4c3aM\r\n",
                "MODE #GSP!worms3!MJ0NJ4c3aM\r\n",
                @"SETCKEY #GPG!622 worms10 :\b_flags\s\r\n",
                @"SETCKEY #GSP!worms3!MJ0NJ4c3aM worms10 :\b_flags\sh"+"\r\n",
                @"GETCKEY #GSP!worms3!MJ0NJ4c3aM * 001 0 :\username\b_flags"+"\r\n",
                "TOPIC #GSP!worms3!MJ0NJ4c3aM :main lobby message test\r\n",
                "MODE #GSP!worms3!MJ0NJ4c3aM +l 2"+"\r\n",
                "PART #GPG!622 :Joined staging room\r\n",
                @"SETCKEY #GSP!worms3!MJ0NJ4c3aM worms10 :\b_firewall\1\b_profileid\7\b_ipaddress\\b_publicip\255.255.255.255\b_privateip\192.168.0.145\b_authresponse\\b_gamever\1073\b_val\0\r\n",
                "WHO worms10"+"\r\n",
                @"SETCHANKEY #GSP!worms3!MJ0NJ4c3aM :\b_hostname\\b_hostport\\b_MaxPlayers\2\b_NumPlayers\0\b_SchemeChanging\0\b_gamever\1073\b_gametype\\b_mapname\Random\b_firewall\1\b_publicip\255.255.255.255\b_privateip\192.168.0.145\b_gamemode\openstaging\b_val\0\b_password\1"+"\r\n",
                "PRIVMSG #GSP!worms3!MJ0NJ4c3aM :staging lobby message test"+"\r\n",
                @"UTM #GSP!worms3!MJ0NJ4c3aM :ATS \nick\worms10\msg\2046\|name|Human Team 1|w0|Dave|w1|Patrick|w2|The Morriss|w3|Mike|w4|Tony|w5|Gluckman|skill|0|grave|0|SWeapon|0|flag|71|speech|Classic|InGame|0|player|worms10|alliance|1|wormCount|77111416"+"\r\n",
                @"UTM #GSP!worms3!MJ0NJ4c3aM :RTS \team\Human Team 1\nick\worms10\msg\2174"+"\r\n"
            };
            var client1 = MockObject.CreateClient(port: 1234) as Client;
            // Client.ClientPool.Remove(client1.Connection.RemoteIPEndPoint, out _);
            ClientManager.RemoveClient(client1);
            var remoteClient = client1.GetRemoteClient() as ITestClient;
            ClientManager.AddClient((IClient)remoteClient);
            foreach (var r in request1)
            {
                var count = ClientManager.ClientPool.Count;
                remoteClient.TestReceived(UniSpyEncoding.GetBytes(r));
                count = ClientManager.ClientPool.Count;
            }
            // IChatClient
            var request2 = new List<string>()
            {
                // "CRYPT des 1 worms3\r\n",
                "USRIP\r\n",
                "USER X419pGl4sX|6 127.0.0.1 peerchat.gamespy.com :dd6283e1e349806c20991020e0d6897a\r\n",
                "NICK xiaojiuwo4\r\n",
                "JOIN #GPG!622\r\n" ,
                "MODE #GPG!622\r\n",
                @"GETCKEY #GPG!622 * 000 0 :\username\b_flags"+"\r\n",
                "PING\r\n",
                "PRIVMSG #GPG!622 :hello from the other side\r\n",
                "JOIN #GSP!worms3!MJ0NJ4c3aM\r\n",
                "MODE #GSP!worms3!MJ0NJ4c3aM\r\n",
                @"SETCKEY #GPG!622 xiaojiuwo4 :\b_flags\s"+"\r\n",
                @"SETCKEY #GSP!worms3!MJ0NJ4c3aM xiaojiuwo4 :\b_flags\s"+"\r\n",
                @"GETCKEY #GSP!worms3!MJ0NJ4c3aM * 001 0 :\username\b_flags"+"\r\n",
                // "PART #GPG!622 :Joined staging room\r\n",
                "UTM #GSP!worms3!MJ0NJ4c3aM :APE [01]privateip[02]192.168.0.109[01]publicip[02]255.255.255.255\r\n",
                "WHO xiaojiuwo4\r\n",
                // "PART #GSP!worms3!MJ0NJ4c3aM :Left Game\r\n"
            };
            var client2 = MockObject.CreateClient(port: 1235) as ITestClient;
            foreach (var r in request2)
            {
                var count = ClientManager.ClientPool.Count;
                client2.TestReceived(UniSpyEncoding.GetBytes(r));
                count = ClientManager.ClientPool.Count;
            }
        }
    }
}
