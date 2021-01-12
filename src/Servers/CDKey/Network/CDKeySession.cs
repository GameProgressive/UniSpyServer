using UniSpyLib.Network;
using System.Net;

namespace CDKey.Network
{
    internal sealed class CDKeySession : UDPSessionBase
    {
        public CDKeySession(UDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
