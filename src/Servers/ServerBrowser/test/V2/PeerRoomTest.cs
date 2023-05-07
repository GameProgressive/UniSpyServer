using UniSpy.Server.QueryReport.Aggregate.Redis.Channel;
using Xunit;

namespace UniSpy.Server.ServerBrowser.Test.V2
{
    public class PeerRoomTest
    {
        [Fact]
        public void TestName()
        {
            var info = new ChannelInfo()
            {
                Name = "#GPG!622"
            };
            QueryReport.Application.StorageOperation.UpdateChannel(info);
            


        }
    }
}