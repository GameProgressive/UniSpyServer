using System.Net;
using UniSpyLib.Network;

namespace CDKey.Network
{
    internal sealed class CDKeySession : UniSpyUDPSessionBase
    {
        public CDKeySession(UniSpyUDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
