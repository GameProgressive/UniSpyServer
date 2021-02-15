using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;
using NATNegotiation.Handler.SystemHandler;
using System;
using UniSpyLib.Abstraction.Interface;

namespace NATNegotiation.Handler.CmdHandler
{
    internal sealed class InitHandler : NNCmdHandlerBase
    {
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result
        {
            get { return (InitResult)base._result; }
            set { base._result = value; }
        }
        private NatUserInfo _userInfo;
        private string _fullKey;
        public InitHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new InitResult();
        }
        protected override void DataOperation()
        {            //TODO we get user infomation from redis
            _fullKey = NatUserInfoRedisOperator.BuildFullKey(
                                        _session.RemoteIPEndPoint,
                                        _request.PortType,
                                        _request.Cookie);
            _userInfo = NatUserInfoRedisOperator.GetSpecificValue(_fullKey);

            if (_userInfo == null)
            {
                _userInfo = new NatUserInfo();
                _userInfo.RemoteEndPoint = _session.RemoteIPEndPoint;
            }
            _userInfo.InitRequestInfo = _request;
            _userInfo.LastPacketRecieveTime = DateTime.Now;
            NatUserInfoRedisOperator.SetKeyValue(_fullKey, _userInfo);
            _result.LocalEndPoint = _session.RemoteEndPoint;
        }

        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}
