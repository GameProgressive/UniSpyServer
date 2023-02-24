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
using UniSpy.Server.Core.Database.DatabaseModel;
using System.Linq;

namespace UniSpy.Server.Chat.Test
{
    public class RedisChatChannelTest
    {
        [Fact]
        public void JsonSerialization()
        {

            var client = TestClasses.CreateClient() as Client;

            client.Info.IsLoggedIn = true;
            client.Info.NickName = "xiaojiuwo";
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
            var client = TestClasses.CreateClient() as Client;

            client.Info.IsLoggedIn = true;
            client.Info.NickName = "xiaojiuwo";
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
    }
}
