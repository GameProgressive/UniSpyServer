using System.Net;
using Xunit;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using WebServer.Handler.CmdHandler;
using UniSpyServer.WebServer.Entity.Structure.Request.SakeRequest;

namespace UniSpyServer.Servers.WebServer.Test.Sake
{
    public class TestSession : IUniSpySession
    {
        public EndPoint RemoteEndPoint => null;
        public IPEndPoint RemoteIPEndPoint => null;

        public bool BaseSend(IUniSpyResponse response)
        {
            return true;
        }

        public bool Send(IUniSpyResponse response)
        {
            return true;
        }
    }
    public class SakeHandlerTest
    {
        private static readonly TestSession _session = new TestSession();
        [Fact]
        public void CreateRecordTest()
        {
            var request = new CreateRecordRequest(SakeRequests.CreateRecord);
            var handler = new CreateRecordHandler(_session, request);
            handler.Handle();
            //When

            //Then
        }
    }
}