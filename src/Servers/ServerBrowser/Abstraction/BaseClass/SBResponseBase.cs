using System;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class SBResponseBase : UniSpyResponseBase
    {
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            set => base.SendingBuffer = value;
        }

        protected SBResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
