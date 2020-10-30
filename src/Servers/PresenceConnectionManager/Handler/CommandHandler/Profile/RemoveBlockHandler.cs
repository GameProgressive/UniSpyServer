using GameSpyLib.Abstraction.Interface;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Abstraction.BaseClass.Profile
{
    public class RemoveBlockHandler : PCMCommandHandlerBase
    {
        public RemoveBlockHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
            throw new NotImplementedException();
        }
    }
}
