using System.Net;
using System.Linq;
using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Application
{
    public class ClientManager : ClientManagerBase
    {
        public static Client GetClient(int profileid, int? productid = null, int? namespaceId = null)
        {
            return (Client)ClientPool.Values.FirstOrDefault(
                c => ((ClientInfo)c.Info).SubProfileInfo.Productid == productid
                && ((ClientInfo)c.Info).SubProfileInfo.Profileid == profileid
                && ((ClientInfo)c.Info).SubProfileInfo.Namespaceid == namespaceId);
        }
    }
}