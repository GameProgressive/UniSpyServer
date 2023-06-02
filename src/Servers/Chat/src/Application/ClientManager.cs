using System.Net;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Application
{
    public sealed class ClientManager : ClientManagerBase<IPEndPoint, IChatClient>
    {
        /// <summary>
        /// We need to make sure client is get by nickname, otherwise we throw exception
        /// </summary>
        /// <param name="nickName"></param>
        public static IChatClient GetClientByNickName(string nickName)
        {
            IChatClient client;
            client = ClientPool.Values.Where(c => c.Info.NickName == nickName).FirstOrDefault();
            return client;
        }
        public static List<ClientInfo> GetAllClientInfo()
        {
            var infos = ClientPool.Values.Select(c => (c.Info)).ToList();
            return infos;
        }
    }
}