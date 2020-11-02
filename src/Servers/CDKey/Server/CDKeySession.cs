using UniSpyLib.Network;
using System.Net;

namespace CDKey.Server
{
    public class CDKeySession : UDPSessionBase
    {
        public CDKeySession(UDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
