using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class SearchUniqueResponse : ResponseBase
    {
        private new SearchUniqueResult _result => (SearchUniqueResult)base._result;

        public SearchUniqueResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\bsr";
            foreach (var info in _result.DatabaseResults)
            {
                SendingBuffer += info.Profileid.ToString();
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
