using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Exception;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Application
{
    public sealed class ClientManager : ClientManagerBase
    {
        public static readonly ConcurrentDictionary<string, RemoteClient> RemoteClientPool = new ConcurrentDictionary<string, RemoteClient>();
        private static string RemoteClientDictionaryKey(RemoteClient client)
        {
            return $"{client.Server.Id} {client.Connection.RemoteIPEndPoint}";
        }
        public static new void AddClient(IClient client)
        {
            if (((IChatClient)client).Info.IsRemoteClient)
            {
                var key = RemoteClientDictionaryKey((RemoteClient)client);
                RemoteClientPool.TryAdd(key, (RemoteClient)client);
            }
            else
            {
                ClientManagerBase.AddClient(client);
            }
        }
        public static new IClient RemoveClient(IClient client)
        {
            if (((IChatClient)client).Info.IsRemoteClient)
            {
                var key = RemoteClientDictionaryKey((RemoteClient)client);
                RemoteClientPool.TryRemove(key, out _);
                return client;
            }
            else
            {
                return ClientManagerBase.RemoveClient(client);
            }
        }
        public static IClient GetClient(IClient client)
        {
            if (((IChatClient)client).Info.IsRemoteClient)
            {
                var key = RemoteClientDictionaryKey((RemoteClient)client);
                if (RemoteClientPool.TryGetValue(key, out var c))
                {
                    return c;

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return ClientManagerBase.GetClient(client.Connection.RemoteIPEndPoint);
            }
        }
        /// <summary>
        /// We need to make sure client is get by nickname, otherwise we throw exception
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public static IChatClient GetClientByNickName(string nickName)
        {
            IChatClient client;
            client = (IChatClient)ClientPool.Values.Where(c => ((ClientInfo)(c.Info)).NickName == nickName).FirstOrDefault();
            if (client is null)
            {
                client = (IChatClient)RemoteClientPool.Values.Where(c => ((ClientInfo)(c.Info)).NickName == nickName).FirstOrDefault();
            }
            if (client is null)
            {
                throw new ChatException($"No client named {nickName} found.");
            }
            return client;
        }
        public static List<ClientInfo> GetAllClientInfo()
        {
            var infos = ClientPool.Values.Select(c => ((ClientInfo)(c.Info))).ToList();
            infos.AddRange(RemoteClientPool.Values.Select(c => ((ClientInfo)(c.Info))).ToList());
            return infos;
        }
    }
}