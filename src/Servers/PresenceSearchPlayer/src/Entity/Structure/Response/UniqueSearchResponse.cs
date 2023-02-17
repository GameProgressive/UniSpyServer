using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class UniqueSearchResponse : ResponseBase
    {
        private new UniqueSearchResult _result => (UniqueSearchResult)base._result;
        private new UniqueSearchRequest _request => (UniqueSearchRequest)base._request;
        public UniqueSearchResponse(RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
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
