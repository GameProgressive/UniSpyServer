using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class ResponseBase : UniSpyResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result => (ResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
        protected ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            List<byte> data = new List<byte>();
            data.AddRange(RequestBase.MagicData);
            data.Add((byte)_request.CommandName);
            data.AddRange(BitConverter.GetBytes(_request.InstantKey));
            SendingBuffer = data.ToArray();
        }
    }
}
