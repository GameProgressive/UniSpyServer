using UniSpyServer.QueryReport.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.QueryReport.Entity.Structure.Response
{
    public sealed class ChallengeResponse : ResponseBase
    {
        private static string Message = "RetroSpy echo!";

        public ChallengeResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            base.Build();
            List<byte> data = new List<byte>();

            data.AddRange(SendingBuffer);
            data.AddRange(UniSpyEncoding.GetBytes(Message));

            SendingBuffer = data.ToArray();
        }
    }
}
