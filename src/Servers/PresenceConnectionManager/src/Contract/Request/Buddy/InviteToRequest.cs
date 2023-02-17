using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Request
{
    /// <summary>
    /// Invite a user to a room or a game
    /// </summary>

    public sealed class InviteToRequest : RequestBase
    {
        public int ProductID { get; private set; }
        public int ProfileId { get; private set; }
        public InviteToRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("productid"))
            {
                throw new GPParseException("productid is missing.");
            }

            if (!RequestKeyValues.ContainsKey("sesskey"))
            {
                throw new GPParseException("sesskey is missing.");

            }

            int productID;
            if (!int.TryParse(RequestKeyValues["productid"], out productID))
            {
                throw new GPParseException("productid format is incorrect.");
            }

            ProductID = productID;

            int profileID;
            if (!int.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid format is incorrect.");
            }
            ProfileId = profileID;
        }
    }
}
