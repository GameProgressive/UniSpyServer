using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using UniSpyLib.Logging;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class QRResponseBase : UniSpyResponseBase
    {
        private new QRRequestBase _request => (QRRequestBase)base._request;
        private new QRResultBase _result => (QRResultBase)base._result;
        protected QRResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            if (_result.ErrorCode != QRErrorCode.NoError)
            {
                BuildErrorResponse();
            }
            else
            {
                BuildNormalResponse();
            }
        }
        protected override void BuildErrorResponse()
        {
            LogWriter.ToLog();
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
