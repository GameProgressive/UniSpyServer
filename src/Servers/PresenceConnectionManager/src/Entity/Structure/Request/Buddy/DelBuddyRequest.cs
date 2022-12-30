using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request
{
    /// <summary>
    /// Delete a user from my friend list
    /// </summary>

    public sealed class DelBuddyRequest : RequestBase
    {
        //\delbuddy\\sesskey\<>\delprofileid\<>\final\
        /// <summary>
        /// The target friendId needs to delete
        /// </summary>
        /// <value></value>
        public int TargetId { get; private set; }
        public DelBuddyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("delprofileid"))
            {
                throw new GPParseException("delprofileid is missing.");
            }

            int deleteProfileID;
            if (!int.TryParse(RequestKeyValues["delprofileid"], out deleteProfileID))
            {
                throw new GPParseException("delprofileid format is incorrect.");
            }

            TargetId = deleteProfileID;
        }
    }
}
