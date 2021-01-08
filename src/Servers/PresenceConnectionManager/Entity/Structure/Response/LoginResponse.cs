using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Result;
using PresenceConnectionManager.Structure.Data;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    public class LoginResponse : PCMResponseBase
    {
        protected new LogInResult _result => (LogInResult)base._result;
        public LoginResponse(UniSpyResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
        {
            //string checkSumStr = _result.DatabaseResults.Nick + _result.DatabaseResults.UniqueNick + _result.DatabaseResults.NamespaceID;

            //_session.UserData.SessionKey = _crc.ComputeChecksum(checkSumStr);

            SendingBuffer = @"\lc\2\sesskey\" + PCMUserInfo.SessionKey;
            SendingBuffer += @"\proof\" + _result.ResponseProof;
            SendingBuffer += @"\userid\" + _result.UserInfo.UserID;
            SendingBuffer += @"\profileid\" + _result.UserInfo.ProfileID;

            if (_result.Request.LoginType != LoginType.NickEmail)
            {
                SendingBuffer += @"\uniquenick\" + _result.UserInfo.UniqueNick;
            }
            SendingBuffer += $@"\lt\{PCMUserInfo.LoginTicket}";
            SendingBuffer += $@"\id\{_result.Request.OperationID}\final\";

            _result.UserInfo.LoginStatus = LoginStatus.Completed;
        }
    }
}
