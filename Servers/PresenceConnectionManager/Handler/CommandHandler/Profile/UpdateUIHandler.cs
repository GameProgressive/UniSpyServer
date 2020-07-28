using GameSpyLib.Common.Entity.Interface;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.CommandHandler.Profile
{
    /// <summary>
    /// Update user information (email)
    /// </summary>
    public class UpdateUIHandler : PCMCommandHandlerBase
    {
        public UpdateUIHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
            //todo find what data is belong to user info
            throw new System.NotImplementedException();
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
        }
    }
}
