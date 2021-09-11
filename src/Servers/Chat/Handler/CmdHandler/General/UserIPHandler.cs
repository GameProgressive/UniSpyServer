﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    [HandlerContract("USRIP")]
    internal sealed class UserIPHandler : CmdHandlerBase
    {

        private new UserIPRequest _request => (UserIPRequest)base._request;
        private new UserIPResult _result
        {
            get => (UserIPResult)base._result;
            set => base._result = value;
        }
        public UserIPHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new UserIPResult();
        }

        protected override void DataOperation()
        {
            _result.RemoteIPAddress = _session.RemoteIPEndPoint.Address.ToString();

        }
        protected override void ResponseConstruct()
        {
            _response = new UserIPResponse(_request, _result);
        }
    }
}
