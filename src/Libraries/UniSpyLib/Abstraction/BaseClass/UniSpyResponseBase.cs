using System;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResponseBase : IUniSpyResponse
    {
        public object ErrorCode { get; protected set; }
        public object SendingBuffer { get; protected set; }
        protected object _result;
        public UniSpyResponseBase(object result)
        {
            _result = result;
            LogWriter.LogCurrentClass(this);
        }

        /// <summary>
        /// Build response message
        /// </summary>
        /// <returns>respnse message</returns>
        public abstract void Build();
    }
}
