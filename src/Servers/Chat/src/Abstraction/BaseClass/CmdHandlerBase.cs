using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
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
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new IShareClient _client => (IShareClient)base._client;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected new ResponseBase _response { get => (ResponseBase)base._response; set => base._response = value; }
        public CmdHandlerBase(IShareClient client, IRequest request) : base(client, request) { }
        protected override void HandleException(System.Exception ex)
        {
            if (ex is IRCException)
            {
                _client.Send(((IRCException)ex));
            }
            base.HandleException(ex);
        }
        protected override void Response()
        {
            if (_client.Info.IsQuietMode)
            {
                return;
            }
            base.Response();
        }
    }
}
