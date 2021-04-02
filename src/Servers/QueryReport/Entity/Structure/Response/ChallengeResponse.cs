using QueryReport.Abstraction.BaseClass;
using System.Collections.Generic;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Encryption;

namespace QueryReport.Entity.Structure.Response
{
    internal sealed class ChallengeResponse : QRResponseBase
    {
        private static string Message = "RetroSpy echo!";

        public ChallengeResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            List<byte> data = new List<byte>();

            data.AddRange(SendingBuffer);
            data.AddRange(UniSpyEncoding.GetBytes(Message));

            SendingBuffer = data.ToArray();
        }
    }
}
