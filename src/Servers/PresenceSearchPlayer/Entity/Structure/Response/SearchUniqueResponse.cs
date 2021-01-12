using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal class SearchUniqueResponse : PSPResponseBase
    {
        protected new SearchUniqueResult _result => (SearchUniqueResult)base._result;

        public SearchUniqueResponse(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
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
