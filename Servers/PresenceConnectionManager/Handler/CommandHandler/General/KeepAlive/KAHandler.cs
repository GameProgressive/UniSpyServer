using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.General.KeepAlive
{
    public class KAHandler : GPCMHandlerBase
    {
        protected KAHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void ConstructResponse(GPCMSession session)
        {
            _sendingBuffer = @"\ka\\final\";
        }
    }
}
