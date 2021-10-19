using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class UniqueSearchResponse : ResponseBase
    {
        private new UniqueSearchResult _result => (UniqueSearchResult)base._result;
        private new UniqueSearchRequest _request => (UniqueSearchRequest)base._request;
        public UniqueSearchResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }


        public override void Build()
        {
            if (_result.IsUniquenickExist)
            {
                SendingBuffer = @"\us\1\nick\Choose another name\usdone\final\";
            }
            else
            {
                SendingBuffer = $@"\us\1\nick\{_request.PreferredNick}\usdone\final\";
            }
        }
    }
}
