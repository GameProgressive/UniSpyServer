﻿using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Contract;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    /// <summary>
    /// Invite a user to a room or a game
    /// </summary>
    [RequestContract("inviteto")]
    internal sealed class InviteToRequest : RequestBase
    {
        public uint ProductID { get; private set; }
        public uint ProfileID { get; private set; }
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

            uint productID;
            if (!uint.TryParse(RequestKeyValues["productid"], out productID))
            {
                throw new GPParseException("productid format is incorrect.");
            }

            ProductID = productID;

            uint profileID;
            if (!uint.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid format is incorrect.");
            }
            ProfileID = profileID;
        }
    }
}