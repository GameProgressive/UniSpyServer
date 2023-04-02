using System.Collections.Generic;
using UniSpy.Server.ServerBrowser.V2.Contract.Result;

namespace UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass
{
    public abstract class AdHocResponseBase : ResponseBase
    {
        public new AdHocResult _result => (AdHocResult)base._result;
        protected List<byte> _buffer = new List<byte>();

        protected AdHocResponseBase(ResultBase result) : base(null, result)
        {
        }
    }
}