using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public class UniqueSearchResponse : PSPResponseBase
    {
        protected new UniqueSearchResult _result => (UniqueSearchResult)base._result;
        public UniqueSearchResponse(PSPResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
        {
            if (!_result.IsUniquenickExist)
            {
                SendingBuffer = @"us\0\usdone\final";
            }
            else
            {
                SendingBuffer = @"\us\1\nick\Choose another name\usdone\final\";
            }
        }
    }
}
