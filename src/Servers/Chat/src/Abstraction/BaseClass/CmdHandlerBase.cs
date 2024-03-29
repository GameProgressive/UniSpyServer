using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

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
        protected new IChatClient _client => (IChatClient)base._client;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected new ResponseBase _response { get => (ResponseBase)base._response; set => base._response = value; }
        public CmdHandlerBase(IChatClient client, IRequest request) : base(client, request) { }
        //if we use this structure the error response should also write to _sendingBuffer
        protected override void HandleException(System.Exception ex)
        {
            if (ex is IRCException)
            {
                _client.Send(((IRCException)ex));
            }
            else if (ex is HandleLaterException)
            {
                // if the exception is HandleLaterException, we log it as warning
                _client.LogWarn(ex.Message);
                return;
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
        /// <summary>
        /// publish message to redis channel
        /// </summary>
        protected virtual void PublishMessage()
        {
            // we do not publish message when the message is received from remote client
            if (_client.Info.IsRemoteClient)
            {
                return;
            }
            // if rawrequest is null means the request is generated by us, we do not publish custom requests
            if (_request.RawRequest is null)
            {
                return;
            }
            var msg = new RemoteMessage(_request, _client.GetRemoteClient());
            Application.Server.GeneralChannel.PublishMessage(msg);
        }
        public override void Handle()
        {
            base.Handle();
            try
            {
                // we publish this message to redis channel
                PublishMessage();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}
