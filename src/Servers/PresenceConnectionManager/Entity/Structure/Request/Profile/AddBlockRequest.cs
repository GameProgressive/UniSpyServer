using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Exception.General;


namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal sealed class AddBlockRequest : PCMRequestBase
    {
        public uint ProfileID;
        public AddBlockRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();



            if (!KeyValues.ContainsKey("profileid"))
            {
                throw new GPParseException("profileid is missing");

            }

            uint profileID;
            if (!uint.TryParse(KeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid format is incorrect");
            }

            ProfileID = profileID;

        }
    }
}
