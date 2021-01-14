using System.Net;
using UniSpyLib.Network;

namespace CDKey.Network
{
    internal sealed class CDKeySession : UDPSessionBase
    {
        public CDKeySession(UDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
