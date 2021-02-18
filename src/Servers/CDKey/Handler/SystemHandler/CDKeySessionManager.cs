using System.Net;
using CDKey.Network;
using UniSpyLib.Abstraction.BaseClass;

namespace CDKey.Handler.SystemHandler
{
    internal sealed class CDKeySessionManager : UniSpySessionManagerBase
    {
        public CDKeySessionManager()
        {
        }

        public CDKeySession GetSession(IPEndPoint endPoint)
        {
            return (CDKeySession)base.GetSession(endPoint);
        }
        public bool AddSession(IPEndPoint endPoint,CDKeySession session)
        {
            return base.AddSession(endPoint, session);
        }
        public bool DeleteSession(IPEndPoint endPoint)
        {
            return base.DeleteSession(endPoint);
        }
    }
}
