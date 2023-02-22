using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Response
{
    public sealed class SearchResponse : ResponseBase
    {
        private new SearchResult _result => (SearchResult)base._result;

        public SearchResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }


        public override void Build()
        {
            SendingBuffer = @"\bsr\";
            foreach (var info in _result.DataBaseResults)
            {
                SendingBuffer += info.ProfileId.ToString();
                SendingBuffer += @"\nick\" + info.Nick;
                SendingBuffer += @"\uniquenick\" + info.Uniquenick;
                SendingBuffer += @"\namespaceid\" + info.NamespaceID;
                SendingBuffer += @"\firstname\" + info.Firstname;
                SendingBuffer += @"\lastname\" + info.Lastname;
                SendingBuffer += @"\email\" + info.Email;
            }
            //TODO check how many user data is allowed send to the client
            SendingBuffer += @"\bsrdone\\more\0\final\";
        }
    }
}
