using System.Linq;
using UniSpyServer.Servers.ServerBrowser.V2.Abstraction;

namespace UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Request
{


    public class SendMsgRequest : AdHocRequest
    {
        public byte[] PrefixMessage { get; private set; }
        public byte[] ClientMessage { get; private set; }
        public SendMsgRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            ClientMessage = RawRequest.Skip(9).ToArray();
        }
    }
}