using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate;
using Xunit;

namespace UniSpy.Server.ServerBrowser.Test.V2
{
    public class PeerRoomTest
    {
        [Fact]
        public void TestName()
        {
            var client = Chat.Test.MockObject.CreateClient();
            var chan = new Channel("Experts", (IShareClient)client);
            Chat.Application.StorageOperation.Persistance.UpdateChannel(chan);
        }
    }
}