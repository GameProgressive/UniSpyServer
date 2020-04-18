using GameSpyLib.Common.Entity.Interface;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.General.KeepAlive
{
    public class KAHandler : PCMCommandHandlerBase
    {
        public KAHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = @"\ka\\final\";
        }
    }
}
