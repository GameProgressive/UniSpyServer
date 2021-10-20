using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Request
{
    /// <summary>
    /// Delete a user from my friend list
    /// </summary>
    [RequestContract("delbuddy")]
    public sealed class DelBuddyRequest : RequestBase
    {
        //\delbuddy\\sesskey\<>\delprofileid\<>\final\
        public uint DeleteProfileID { get; private set; }
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

            uint deleteProfileID;
            if (!uint.TryParse(RequestKeyValues["delprofileid"], out deleteProfileID))
            {
                throw new GPParseException("delprofileid format is incorrect.");
            }

            DeleteProfileID = deleteProfileID;
        }
    }
}
