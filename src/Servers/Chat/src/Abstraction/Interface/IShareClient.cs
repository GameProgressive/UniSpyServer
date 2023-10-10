using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Abstraction.Interface
{
    public interface IShareClient : IClient, ITestClient
    {
        public new ClientInfo Info { get; }
        public bool IsRemoteClient { get; }
        public RemoteClient GetRemoteClient();
    }
}