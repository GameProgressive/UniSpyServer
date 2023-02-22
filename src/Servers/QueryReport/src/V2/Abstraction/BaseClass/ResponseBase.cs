using System;
using System.Collections.Generic;

namespace UniSpy.Server.QueryReport.V2.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpy.Server.Core.Abstraction.BaseClass.ResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result => (ResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
        public ResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            List<byte> data = new List<byte>();
            data.AddRange(RequestBase.MagicData);
            data.Add((byte)_request.CommandName);
            data.AddRange(BitConverter.GetBytes((uint)_request.InstantKey));
            SendingBuffer = data.ToArray();
        }
    }
}
