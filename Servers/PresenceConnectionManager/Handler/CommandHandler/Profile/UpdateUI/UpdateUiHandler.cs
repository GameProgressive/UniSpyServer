using System.Collections.Generic;
using GameSpyLib.Common.Entity.Interface;

namespace PresenceConnectionManager.Handler.Profile.UpdateUI
{
    /// <summary>
    /// Update user information (email)
    /// </summary>
    public class UpdateUIHandler : PCMCommandHandlerBase
    {
        public UpdateUIHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }
    }
}
