using System.Collections.Generic;
using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Response
{
    public sealed class ChallengeResponse : ResponseBase
    {
        private static string Message = "RetroSpy echo!";

        public ChallengeResponse(ChallengeRequest request, ChallengeResult result) : base(request, result)
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
