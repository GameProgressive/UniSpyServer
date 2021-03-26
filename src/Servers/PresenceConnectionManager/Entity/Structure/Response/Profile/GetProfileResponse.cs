using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.MiscMethod;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal class GetProfileResponse : PCMResponseBase
    {
        protected new GetProfileResult _result => (GetProfileResult)base._result;
        public GetProfileResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
            SendingBuffer = @"\pi\\profileid\" + _result.UserProfile.ProfileID;
            SendingBuffer += @"\nick\" + _result.UserProfile.Nick;
            SendingBuffer += @"\uniquenick\" + _result.UserProfile.UniqueNick;
            SendingBuffer += @"\email\" + _result.UserProfile.Email;
            SendingBuffer += @"\firstname\" + _result.UserProfile.Firstname;
            SendingBuffer += @"\lastname\" + _result.UserProfile.Lastname;
            SendingBuffer += @"\icquin\" + _result.UserProfile.Icquin;
            SendingBuffer += @"\homepage\" + _result.UserProfile.Homepage;
            SendingBuffer += @"\zipcode\" + _result.UserProfile.Zipcode;
            SendingBuffer += @"\countrycode\" + _result.UserProfile.Countrycode;
            SendingBuffer += @"\lon\" + _result.UserProfile.Longitude;
            SendingBuffer += @"\lat\" + _result.UserProfile.Longitude;
            SendingBuffer += @"\loc\" + _result.UserProfile.Location;

            int birthStr = (int)_result.UserProfile.Birthday << 24 | (int)_result.UserProfile.Birthmonth << 16 | (int)_result.UserProfile.Birthyear;

            SendingBuffer += @"\birthday\" + birthStr;
            SendingBuffer += @"\sex\" + _result.UserProfile.Sex;
            SendingBuffer += @"\publicmask\" + _result.UserProfile.Publicmask;
            SendingBuffer += @"\aim\" + _result.UserProfile.Aim;
            SendingBuffer += @"\picture\" + _result.UserProfile.Picture;
            SendingBuffer += @"\ooc" + _result.UserProfile.Occupationid;
            SendingBuffer += @"\ind\" + _result.UserProfile.Industryid;
            SendingBuffer += @"\inc\" + _result.UserProfile.Incomeid;
            SendingBuffer += @"\mar\" + _result.UserProfile.Marriedid;
            SendingBuffer += @"\chc\" + _result.UserProfile.Childcount;
            SendingBuffer += @"\i1\" + _result.UserProfile.Interests1;
            SendingBuffer += @"\o1\" + _result.UserProfile.Ownership1;
            SendingBuffer += @"\conn\" + _result.UserProfile.Connectiontype;
            // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
            SendingBuffer += @"\sig\+" + GameSpyRandom.GenerateRandomString(10, GameSpyRandom.StringType.Hex);
            SendingBuffer += @"\id\" + _request.OperationID + @"\final\";
        }
    }
}
