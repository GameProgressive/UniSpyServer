using GameSpyLib.Common.Entity.Interface;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.General.KeepAlive
{
    public class KeepAliveHandler : PCMCommandHandlerBase
    {
        public KeepAliveHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            throw new System.NotImplementedException();
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = @"\ka\\final\";
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
