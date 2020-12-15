using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Network;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.SystemHandler.Manager;

namespace NATNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// because we are using self defined error code so we do not need
    /// to send it to client, when we detect errorCode != noerror we just log it
    /// </summary>
    public class NNCommandHandlerBase : UniSpyCmdHandlerBase
    {
        protected NNErrorCode _errorCode;
        protected byte[] _sendingBuffer;
        protected new NNSession _session
        {
            get { return (NNSession)base._session; }
        }
        protected new NNRequestBase _request
        {
            get { return (NNRequestBase)base._request; }
        }
        protected NatUserInfo _userInfo;
        public NNCommandHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _errorCode = NNErrorCode.NoError;
        }

        public override void Handle()
        {
            CheckRequest();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }

            DataOperation();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }
            ConstructResponse();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }
            Response();
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            //TODO we get user infomation from redis
            var keys = NatUserInfo.GetMatchedKeys(_session.RemoteEndPoint);
            if (keys.Count == 0)
            {
                _userInfo = new NatUserInfo();
                _userInfo.UpdateRemoteEndPoint(_session.RemoteEndPoint);
            }
            else
            {
                _userInfo = NatUserInfo.GetNatUserInfo(_session.RemoteEndPoint, _request.Cookie);
            }
        }

        protected override void Response()
        {
            if (!StringExtensions.CheckResponseValidation(_sendingBuffer))
            {
                return;
            }
            _session.Send(_sendingBuffer);
        }
    }
}
