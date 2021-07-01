﻿using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Application;
using NatNegotiation.Entity.Structure.Redis;
using NatNegotiation.Entity.Structure.Request;
using NatNegotiation.Entity.Structure.Response;
using NatNegotiation.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;
using System;
using UniSpyLib.Abstraction.Interface;

namespace NatNegotiation.Handler.CmdHandler
{
    [Command((byte)0)]
    internal sealed class InitHandler : NNCmdHandlerBase
    {
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result
        {
            get => (InitResult)base._result;
            set => base._result = value;
        }
        private NatUserInfo _userInfo;
        public InitHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new InitResult();
        }
        protected override void DataOperation()
        {
            //TODO we get user infomation from redis
            var fullKey = new NatUserInfoRedisKey()
            {
                ServerID = NNServerFactory.Server.ServerID,
                RemoteIPEndPoint = _session.RemoteIPEndPoint,
                PortType = _request.PortType,
                Cookie = _request.Cookie
            };
            _userInfo = NatUserInfoRedisOperator.GetSpecificValue(fullKey);

            if (_userInfo == null)
            {
                _userInfo = new NatUserInfo();
                _userInfo.RemoteEndPoint = _session.RemoteIPEndPoint;
            }
            _userInfo.InitRequestInfo = _request;
            _userInfo.LastPacketRecieveTime = DateTime.Now;
            NatUserInfoRedisOperator.SetKeyValue(fullKey, _userInfo);
            _result.LocalIPEndPoint = _session.RemoteIPEndPoint;
        }

        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
    }
}