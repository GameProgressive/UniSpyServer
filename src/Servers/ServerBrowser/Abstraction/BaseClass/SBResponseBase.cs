using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class SBResponseBase : UniSpyResponseBase
    {
        protected new SBRequestBase _request => (SBRequestBase)base._request;
        protected new SBResultBase _result => (SBResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            set => base.SendingBuffer = value;
        }
        protected SBResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        /// <summary>
        /// SB always need to response to client even there are no server or error occured
        /// </summary>
        public override void Build()
        {
            BuildNormalResponse();
        }
    }
}
