using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.PresenceConnectionManager.Structure.Data;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Response
{
    public sealed class LoginResponse : ResponseBase
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

            SendingBuffer = @"\lc\2\sesskey\" + UserInfo.SessionKey;
            SendingBuffer += @"\proof\" + _result.ResponseProof;
            SendingBuffer += @"\userid\" + _result.DatabaseResults.UserID;
            SendingBuffer += @"\profileid\" + _result.DatabaseResults.ProfileID;

            if (_request.LoginType != LoginType.NickEmail)
            {
                SendingBuffer += @"\uniquenick\" + _result.DatabaseResults.UniqueNick;
            }
            SendingBuffer += $@"\lt\{UserInfo.LoginTicket}";
            SendingBuffer += $@"\id\{_request.OperationID}\final\";
        }
    }
}
