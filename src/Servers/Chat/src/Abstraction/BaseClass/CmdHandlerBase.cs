using System;
using UniSpyServer.Servers.Chat.Entity;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass
{
    /// <summary>
    /// error code condition is complicated
    /// there are 2 types error code
    /// 1.irc numeric error code
    /// 2.self defined code
    /// we do not want to send self defined code, so if we find errorCode < noerror
    /// we just return.
    /// if error code bigger than noerror we need to process it in ConstructResponse()
    ///we also need to check the error code != noerror in ConstructResponse()
    /// </summary>
    public abstract class CmdHandlerBase : UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResponseBase _response { get => (ResponseBase)base._response; set => base._response = value; }
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(ISession session, IRequest request) : base(session, request)
        {
        }
        //if we use this structure the error response should also write to _sendingBuffer
        protected override void HandleException(Exception ex)
        {
            if (ex is IRCException)
            {
                _session.SendAsync(((IRCException)ex).ErrorResponse);
            }
            else
            {
                base.HandleException(ex);
            }
        }
        protected override void RequestCheck()
        {
            if (_request.RawRequest != null)
            {
                base.RequestCheck();
            }
        }
        protected override void Response()
        {
            if (_session.UserInfo.IsQuietMode)
            {
                return;
            }
            base.Response();
        }
    }
}
