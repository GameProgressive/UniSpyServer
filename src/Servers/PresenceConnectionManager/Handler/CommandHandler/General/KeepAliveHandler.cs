using GameSpyLib.Abstraction.Interface;
using System.Collections.Generic;

namespace PresenceConnectionManager.Abstraction.BaseClass.General
{
    public class KeepAliveHandler : PCMCommandHandlerBase
    {
        public KeepAliveHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            //we need to keep player cache online
            //so their friends can find him
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = @"\ka\\final\";
        }
    }
}
