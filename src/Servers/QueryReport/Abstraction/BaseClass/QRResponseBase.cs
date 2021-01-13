using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class QRResponseBase:UniSpyResponseBase
    {
        private new QRRequestBase _request => (QRRequestBase)base._request;
        private new QRResultBase _result => (QRResultBase)base._result;
        protected QRResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(QRRequestBase.MagicData);
            data.Add((byte)_request.CommandName);
            data.AddRange(BitConverter.GetBytes(_request.InstantKey));

            SendingBuffer = data.ToArray();
        }
    }
}
