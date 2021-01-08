using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    public class AddBuddyResponse : PCMResponseBase
    {
        protected new AddBuddyResult _result => (AddBuddyResult)base._result;

        public AddBuddyResponse(UniSpyResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
        {
            throw new NotImplementedException();
        }
    }
}
