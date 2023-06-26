using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Aggregate;
using UniSpy.Server.PresenceConnectionManager.Application;

namespace UniSpy.Server.PresenceConnectionManager.Abstraction.Interface
{
    public interface IShareClient : IClient, ITestClient
    {
        public new ClientInfo Info { get; }
        public RemoteClient GetRemoteClient();
    }
}