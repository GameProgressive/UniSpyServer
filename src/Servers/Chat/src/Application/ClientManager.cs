using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Application
{
    public sealed class ClientManager : ClientManagerBase
    {
        /// <summary>
        /// We need to make sure client is get by nickname, otherwise we throw exception
        /// </summary>
        public static Client GetClientByNickName(string nickName)
        {
            var client = (Client)ClientPool.Values.FirstOrDefault(c => ((IShareClient)c).Info.NickName == nickName);
            return client;
        }
        public static List<ClientInfo> GetAllClientInfo()
        {
            var infos = ClientPool.Values.Select(c => ((ClientInfo)(c.Info))).ToList();
            return infos;
        }
    }
}