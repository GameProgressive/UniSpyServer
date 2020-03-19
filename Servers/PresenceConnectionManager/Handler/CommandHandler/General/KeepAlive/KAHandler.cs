using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.General.KeepAlive
{
    public class KAHandler : CommandHandlerBase
    {
        protected KAHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
        }

        protected override void ConstructResponse(GPCMSession session, Dictionary<string, string> recv)
        {
            _sendingBuffer = @"\ka\\final\";
        }
    }
}
