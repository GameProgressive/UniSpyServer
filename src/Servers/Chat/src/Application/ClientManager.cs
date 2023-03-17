using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Exception;

namespace UniSpy.Server.Chat.Application
{
    public sealed class ClientManager : Core.Abstraction.BaseClass.ClientManagerBase
    {
        /// <summary>
        /// We need to make sure client is get by nickname, otherwise we throw exception
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public static IChatClient GetClientByNickName(string nickName)
        {
            IChatClient client;
            client = (Client)ClientPool.Values.Where(c => ((ClientInfo)(c.Info)).NickName == nickName).FirstOrDefault();
            if (client is null)
            {
                throw new ChatException($"No client named {nickName} found.");
            }
            return client;
        }
        public static List<ClientInfo> GetAllClientInfo()
        {
            var infos = ClientPool.Values.Select(c => ((ClientInfo)(c.Info))).ToList();
            return infos;
        }
    }
}