using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Exception.General;


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
                throw new GPParseException("delprofileid is missing.");
            }

            uint deleteProfileID;
            if (!uint.TryParse(KeyValues["delprofileid"], out deleteProfileID))
            {
                throw new GPParseException("delprofileid format is incorrect.");
            }

            DeleteProfileID = deleteProfileID;
        }
    }
}
