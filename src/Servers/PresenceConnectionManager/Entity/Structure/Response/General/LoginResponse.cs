using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Entity.Structure.Result;
using PresenceConnectionManager.Structure.Data;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal sealed class LoginResponse : PCMResponseBase
    {
        private new LoginResult _result => (LoginResult)base._result;
        private new LoginRequest _request => (LoginRequest)base._request;
        public LoginResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            //string checkSumStr = _result.DatabaseResults.Nick + _result.DatabaseResults.UniqueNick + _result.DatabaseResults.NamespaceID;

            //_session.UserData.SessionKey = _crc.ComputeChecksum(checkSumStr);

            SendingBuffer = @"\lc\2\sesskey\" + PCMUserInfo.SessionKey;
            SendingBuffer += @"\proof\" + _result.ResponseProof;
            SendingBuffer += @"\userid\" + _result.DatabaseResults.UserID;
            SendingBuffer += @"\profileid\" + _result.DatabaseResults.ProfileID;

            if (_request.LoginType != LoginType.NickEmail)
            {
                SendingBuffer += @"\uniquenick\" + _result.DatabaseResults.UniqueNick;
            }
            SendingBuffer += $@"\lt\{PCMUserInfo.LoginTicket}";
            SendingBuffer += $@"\id\{_request.OperationID}\final\";
        }
    }
}
