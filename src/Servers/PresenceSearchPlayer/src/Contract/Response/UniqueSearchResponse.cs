using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Request;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Response
{
    public sealed class UniqueSearchResponse : ResponseBase
    {
        private new UniqueSearchRequest _request => (UniqueSearchRequest)base._request;
        private new UniqueSearchResult _result => (UniqueSearchResult)base._result;
        public UniqueSearchResponse(UniqueSearchRequest request, UniqueSearchResult result) : base(request, result)
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
