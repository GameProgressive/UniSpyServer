using System;
using UniSpyLib.Abstraction.Interface;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResponseBase : IUniSpyResponse
    {
        protected object _result;
        public UniSpyResponseBase(object result)
        {
            _result = result;
        }

        /// <summary>
        /// Build response message
        /// </summary>
        /// <returns>respnse message</returns>
        public abstract object Build();
    }
}
