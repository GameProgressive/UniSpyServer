using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class QRResponseBase : UniSpyResponse
    {
        protected new QRRequestBase _request => (QRRequestBase)base._request;
        protected new QRResultBase _result => (QRResultBase)base._result;
        public new byte[] SendingBuffer
        {
            get => (byte[])base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
        protected QRResponseBase(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            List<byte> data = new List<byte>();
            data.AddRange(QRRequestBase.MagicData);
            data.Add((byte)_request.CommandName);
            data.AddRange(BitConverter.GetBytes(_request.InstantKey));
            SendingBuffer = data.ToArray();
        }
    }
}
