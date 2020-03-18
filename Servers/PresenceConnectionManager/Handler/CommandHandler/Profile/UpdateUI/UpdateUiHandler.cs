using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Profile.UpdateUI
{
    /// <summary>
    /// Update user information (email)
    /// </summary>
    public class UpdateUIHandler : CommandHandlerBase
    {
        public UpdateUIHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }
    }
}
