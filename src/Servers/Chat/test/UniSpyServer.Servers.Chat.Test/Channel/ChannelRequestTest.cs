using Xunit;
using System.Collections.Generic;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;

namespace UniSpyServer.Servers.Chat.Test.Channel
{
    public class ChannelRequestTest
    {
        [Fact(Skip = "Unit test is broken")]
        public void GetChannelKey()
        {
            var request = new GetChannelKeyRequest(ChannelRequests.GetChannelKey);
            request.Parse();
            Assert.Equal("XXXX", request.Cookie);
        }

        [Fact]
        public void GetCKeyChannelSpecificUser()
        {
            var request = new GetCKeyRequest(ChannelRequests.GetCKeyChannelSpecificUser);
            request.Parse();
            Assert.Equal(GetKeyReqeustType.GetChannelSpecificUserKeyValue, request.RequestType);
            Assert.Equal("spyguy", request.NickName);
            Assert.Equal("000", request.Cookie);
            Assert.Equal("0", request.UnkownCmdParam);
            Assert.Equal("username", request.Keys[0]);
            Assert.Equal("b_flags", request.Keys[1]);
        }

        [Fact]
        public void GetCKeyChannelAllUser()
        {
            var request = new GetCKeyRequest(ChannelRequests.GetCKeyChannelAllUser);
            request.Parse();
            Assert.Equal(GetKeyReqeustType.GetChannelAllUserKeyValue, request.RequestType);
            Assert.Equal("000", request.Cookie);
            Assert.Equal("0", request.UnkownCmdParam);
            Assert.Equal("username", request.Keys[0]);
            Assert.Equal("b_flags", request.Keys[1]);
        }

        [Fact]
        public void Join()
        {
            var request = new JoinRequest(ChannelRequests.Join);
            request.Parse();
            Assert.Equal("#GSP!room!test", request.ChannelName);

            request = new JoinRequest(ChannelRequests.JoinWithPass);
            request.Parse();
            Assert.Equal("#GSP!room!test", request.ChannelName);
            Assert.Equal("pass123", request.Password);
        }

        [Fact]
        public void Kick()
        {
            var request = new KickRequest(ChannelRequests.Kick);
            request.Parse();
            Assert.Equal("spyguy", request.KickeeNickName);
            Assert.Equal("Spam", request.Reason);
        }

        [Fact(Skip = "TODO: add tests")]
        public void Mode()
        {
            var request = new ModeRequest(ChannelRequests.Mode);
            request.Parse();
            Assert.Equal(ModeRequestType.SetChannelModesWithUserLimit, request.RequestType);
            Assert.Equal("#GSP!room!test", request.ChannelName);
            Assert.Equal("+l", request.ModeFlag);
            Assert.Equal((uint)2, request.LimitNumber);
        }

        [Fact]
        public void Part()
        {
            var request = new PartRequest(ChannelRequests.Part);
            request.Parse();
            Assert.Equal("test", request.Reason);
        }

        [Fact(Skip = "TODO: add tests")]
        public void SetChannelKey()
        {
            var request = new SetChannelKeyRequest(ChannelRequests.SetChannelKey);
            request.Parse();
        }

        [Fact]
        public void SetCKey()
        {
            var request = new SetCKeyRequest(ChannelRequests.SetCKey);
            request.Parse();
            Dictionary<string, string> dict1 = new Dictionary<string, string> 
            {
                { 
                    "b_flags", "sh" 
                } 
            };
            Assert.Equal("#GSP!room!test", request.Channel);
            Assert.Equal("spyguy", request.NickName);
            Assert.Equal(dict1, request.KeyValues);
        }

        [Fact]
        public void TopicGetChannelTopic()
        {
            var request = new TopicRequest(ChannelRequests.TopicGetChannelTopic);
            request.Parse();
            Assert.Equal("#GSP!room!test", request.ChannelName);
        }

        [Fact]
        public void TopicSetChannelTopic()
        {
            var request = new TopicRequest(ChannelRequests.TopicSetChannelTopic);
            request.Parse();
            Assert.Equal("#GSP!room!test", request.ChannelName);
            Assert.Equal("This is a topic message.", request.ChannelTopic);
        }
    }
}
