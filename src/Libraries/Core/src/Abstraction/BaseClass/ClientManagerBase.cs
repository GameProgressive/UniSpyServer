using System.Collections.Concurrent;
using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    /// <summary>
    /// This class provide global access to client pool,
    /// if you need search client in specific server,
    /// please inherit this class and create static method.
    /// </summary>
    public abstract class ClientManagerBase
    {
        public static readonly ConcurrentDictionary<IPEndPoint, IClient> ClientPool = new ConcurrentDictionary<IPEndPoint, IClient>();
        public static void AddClient(IClient client)
        {
            ClientPool.TryAdd(client.Connection.RemoteIPEndPoint, client);
        }
        public static IClient RemoveClient(IClient client)
        {
            return RemoveClient(client.Connection.RemoteIPEndPoint);
        }
        public static IClient RemoveClient(IPEndPoint endPoint)
        {
            ClientPool.TryRemove(endPoint, out var client);
            return client;
        }
        public static IClient GetClient(IPEndPoint endPoint)
        {
            if (ClientPool.TryGetValue(endPoint, out var client))
            {
                return client;
            }
            else
            {
                return default;
            }
        }
    }
}