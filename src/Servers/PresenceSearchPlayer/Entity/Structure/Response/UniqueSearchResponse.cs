using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal class UniqueSearchResponse : PSPResponseBase
    {
        public UniqueSearchResponse(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected new UniqueSearchResult _result => (UniqueSearchResult)base._result;

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
