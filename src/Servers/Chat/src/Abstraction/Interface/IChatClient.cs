using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Abstraction.Interface
{
    public interface IChatClient : IClient
    {
        public new ClientInfo Info { get; }
        public RemoteClient GetRemoteClient();
    }
}