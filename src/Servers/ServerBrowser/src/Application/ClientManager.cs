using UniSpy.Server.Core.Abstraction.BaseClass;
using System.Linq;
using System.Collections.Generic;

namespace UniSpy.Server.ServerBrowser.Application
{
    public class ClientManager : ClientManagerBase
    {
        public static List<Client> GetClient(string gameName)
        {
            return ClientPool.Values.Where(c=>(((Client)c).Info).GameName == gameName).Select(c=>((Client)c)).ToList();
        }
    }
}