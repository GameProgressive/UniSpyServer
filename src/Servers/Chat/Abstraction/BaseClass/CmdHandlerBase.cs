using System;
using Chat.Entity.Exception.IRC.General;
using Chat.Entity.Structure.Response;
using Chat.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Abstraction.BaseClass
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
    internal abstract class CmdHandlerBase : UniSpyLib.Abstraction.BaseClass.UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResponseBase _response { get => (ResponseBase)base._response; set => base._response = value; }
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        //if we use this structure the error response should also write to _sendingBuffer
        protected override void HandleException(Exception ex)
        {
            if (ex is ChatIRCException)
            {
                _session.SendAsync(((ChatIRCException)ex).ErrorResponse);
            }
            else
            {
                base.HandleException(ex);
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
