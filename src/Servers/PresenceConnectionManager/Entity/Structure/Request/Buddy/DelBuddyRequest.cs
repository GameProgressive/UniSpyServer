using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal sealed class DelBuddyRequest : PCMRequestBase
    {
        //\delbuddy\\sesskey\<>\delprofileid\<>\final\
        public uint DeleteProfileID { get; private set; }
        public DelBuddyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!KeyValues.ContainsKey("delprofileid"))
            {
                throw new GPGeneralException("delprofileid is missing.", GPErrorCode.Parse);
            }

            uint deleteProfileID;
            if (!uint.TryParse(KeyValues["delprofileid"], out deleteProfileID))
            {
                throw new GPGeneralException("delprofileid format is incorrect.", GPErrorCode.Parse);
            }

            DeleteProfileID = deleteProfileID;
        }
    }
}
