using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResponseBase : IUniSpyResponse
    {
        public object ErrorCode { get; protected set; }
        public object SendingBuffer { get; protected set; }
        protected object _dataResult;
        public UniSpyResponseBase(object dataResult)
        {
            _dataResult = dataResult;
            LogWriter.LogCurrentClass(this);
        }

        /// <summary>
        /// Build response message
        /// </summary>
        public abstract void Build();
    }
}
