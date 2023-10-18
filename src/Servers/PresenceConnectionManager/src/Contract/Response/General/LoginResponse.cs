using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Enumerate;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class LoginResponse : ResponseBase
    {
        private new LoginRequest _request => (LoginRequest)base._request;
        private new LoginResult _result => (LoginResult)base._result;
        public LoginResponse(LoginRequest request, LoginResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            //string checkSumStr = _result.DatabaseResults.Nick + _result.DatabaseResults.UniqueNick + _result.DatabaseResults.NamespaceID;

            //_connection.UserData.SessionKey = _crc.ComputeChecksum(checkSumStr);

            SendingBuffer = @"\lc\2\sesskey\" + ClientInfo.SessionKey;
            SendingBuffer += @"\proof\" + _result.ResponseProof;
            SendingBuffer += @"\userid\" + _result.DatabaseResults.UserId;
            SendingBuffer += @"\profileid\" + _result.DatabaseResults.ProfileId;

            if (_result.DatabaseResults.UniqueNick is not null)
            {
                SendingBuffer += @"\uniquenick\" + _result.DatabaseResults.UniqueNick;
            }
            SendingBuffer += $@"\lt\{ClientInfo.LoginTicket}";
            SendingBuffer += $@"\id\{_request.OperationID}\final\";
        }
    }
}
