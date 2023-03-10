using System.Linq;
using UniSpy.Server.ServerBrowser.Abstraction;

namespace UniSpy.Server.ServerBrowser.Contract.Request
{


    public class SendMsgRequest : AdHocRequestBase
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