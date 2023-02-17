using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class NicksResponse : ResponseBase
    {
        private new NicksResult _result => (NicksResult)base._result;
        private new NicksRequest _request => (NicksRequest)base._request;

        public NicksResponse(RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\nr\";
            foreach (var info in _result.DataBaseResults)
            {
                SendingBuffer += @"\nick\";
                SendingBuffer += info.NickName;
                if (_request.IsRequireUniqueNicks)
                {
                    SendingBuffer += @"\uniquenick\";
                    SendingBuffer += info.UniqueNick;
                }
            }
            SendingBuffer += @"\ndone\final\";
        }
    }
}
