using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void HandleException(System.Exception ex)
        {
            if (ex is GPException)
            {
                _client.Send(((GPException)ex));
            }
            base.HandleException(ex);
        }
    }
}
