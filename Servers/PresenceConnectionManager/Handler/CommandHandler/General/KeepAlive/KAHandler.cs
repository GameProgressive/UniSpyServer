using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.General.KeepAlive
{
    public class KAHandler : GPCMHandlerBase
    {
        protected KAHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void ConstructResponse(GPCMSession session, Dictionary<string, string> recv)
        {
            _sendingBuffer = @"\ka\\final\";
        }
    }
}
