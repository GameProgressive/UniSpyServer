using System;
using System.Linq;
using UniSpyServer.Servers.ServerBrowser.V2.Abstraction;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Exception;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Request
{


    public class SendMsgRequest : AdHocRequest
    {
        public byte[] PrefixMessage { get; private set; }
        public byte[] PostfixMessage { get; private set; }
        public SendMsgRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            if (((RequestType)RawRequest[2]) != RequestType.SendMessageRequest)
            {
                PostfixMessage = RawRequest;
            }
            else
            {
                base.Parse();
                if (RawRequest.Length > 9)
                {
                    PostfixMessage = RawRequest.Skip(9).ToArray();
                    PrefixMessage = RawRequest.Take(9).ToArray();
                }
                else
                {
                    LogWriter.LogWarn("send message request incomplete.");
                }
            }
        }
    }
}