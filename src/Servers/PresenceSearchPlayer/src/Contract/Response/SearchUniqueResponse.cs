using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Response
{
    public sealed class SearchUniqueResponse : ResponseBase
    {
        private new SearchUniqueResult _result => (SearchUniqueResult)base._result;

        public SearchUniqueResponse(RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\bsr";
            foreach (var info in _result.DatabaseResults)
            {
                SendingBuffer += info.ProfileId.ToString();
                SendingBuffer += @"\nick\" + info.Nick;
                SendingBuffer += @"\uniquenick\" + info.Uniquenick;
                SendingBuffer += @"\namespaceid\" + info.NamespaceID;
                SendingBuffer += @"\firstname\" + info.Firstname;
                SendingBuffer += @"\lastname\" + info.Lastname;
                SendingBuffer += @"\email\" + info.Email;
            }
            SendingBuffer += @"\bsrdone\\more\0";
        }
    }
}
