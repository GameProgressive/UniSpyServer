using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    internal sealed class GetProfileRequest : PCMRequestBase
    {
        public uint ProfileID { get; private set; }
        public GetProfileRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if( ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!KeyValues.ContainsKey("profileid"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }

            uint profileID;
            if (!uint.TryParse(KeyValues["profileid"], out profileID))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }
            ProfileID = profileID;

            if (!KeyValues.ContainsKey("sesskey"))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }
        }
    }
}
