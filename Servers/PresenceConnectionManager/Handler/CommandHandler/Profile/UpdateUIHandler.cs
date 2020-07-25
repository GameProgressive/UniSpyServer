using GameSpyLib.Common.Entity.Interface;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Profile.UpdateUI
{
    /// <summary>
    /// Update user information (email)
    /// </summary>
    public class UpdateUIHandler : PCMCommandHandlerBase
    {
        public UpdateUIHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }
    }
}
