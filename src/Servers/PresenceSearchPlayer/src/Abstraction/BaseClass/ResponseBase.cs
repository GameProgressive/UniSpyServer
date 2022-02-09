﻿using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpyLib.Abstraction.BaseClass.ResponseBase
    {
        protected new ResultBase _result => (ResultBase)base._result;
        protected new RequestBase _request => (RequestBase)base._request;
        public new string SendingBuffer { get => (string)base.SendingBuffer; protected set => base.SendingBuffer = value; }
        protected ResponseBase(RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
    }
}
