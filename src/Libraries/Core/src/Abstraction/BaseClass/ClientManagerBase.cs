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
    public abstract class ClientManagerBase<TKey, TValue>
                                where TKey : IPEndPoint
                                where TValue : IClient
    {
        public static readonly ConcurrentDictionary<TKey, TValue> ClientPool = new ConcurrentDictionary<TKey, TValue>();
        public static void AddClient(TValue client)
        {
            ClientPool.TryAdd((TKey)client.Connection.RemoteIPEndPoint, client);
        }
        public static TValue RemoveClient(TValue client)
        {
            return RemoveClient((TKey)client.Connection.RemoteIPEndPoint);
        }
        public static TValue RemoveClient(TKey endPoint)
        {
            ClientPool.TryRemove(endPoint, out var client);
            return client;
        }
        public static TValue GetClient(TKey endPoint)
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