using Moq;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Network.Http.Server;
using UniSpy.Server.WebServer.Handler;
using Xunit;

namespace UniSpy.Server.WebServer.Test
{
    public class GeneralTest
    {
        [Fact]
        public void InlegalMessageTest()
        {
            var client = MokeObject.CreateClient();
            var req = new Mock<IHttpRequest>();
            req.Setup(s => s.Body).Returns("username=admin&psd=Feefifofum");
            req.Setup(s => s.Url).Returns("abcdefg");
            var switcher = new CmdSwitcher(client, req.Object);
            switcher.Handle();
        }

        [Fact]
        public void BufferCacheTest()
        {
            // Given
            var req1 = "POST /AuthService/AuthService.asmx HTTP/1.1\r\nHost: capricorn.auth.pubsvs.gamespy.com\r\nUser-Agent: GameSpyHTTP/1.0\r\nConnection: close\r\nContent-Length: 861\r\nContent-Type: text/xml\r\nSOAPAction: \"http://gamespy.net/AuthService/LoginUniqueNick\"\r\n";
            var req2 = "\r\n<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:ns1=\"http://gamespy.net/AuthService/\"><SOAP-ENV:Body><ns1:LoginUniqueNick><ns1:version>1</ns1:version><ns1:partnercode>95</ns1:partnercode><ns1:namespaceid>95</ns1:namespaceid><ns1:uniquenick>asdf21</ns1:uniquenick><ns1:password><ns1:Value>0002899d0429a3bc5c24934320e526bed7ad645fb56c7783e2c8afef77e0dd1005d3383b58eb6919baae85881183148a4b4ebe1169475c85e305a2ca4f2d30d9ecf2275dda0706a14d15c5eb755bbca034168b13997f9111ebe821d640f64622586f231479a8516d47c046a819b19e03ba4697c68e3879dafab9380032333233</ns1:Value></ns1:password></ns1:LoginUniqueNick></SOAP-ENV:Body></SOAP-ENV:Envelope>";
            // When
            var cache = new HttpBufferCache();
            string completeBuffer;
            Assert.False(cache.ProcessBuffer(req1, out completeBuffer));

            Assert.True(cache.ProcessBuffer(req2, out completeBuffer));

            Assert.True(cache.InCompleteBuffer == null);
            var resultreq = "POST /AuthService/AuthService.asmx HTTP/1.1\r\nHost: capricorn.auth.pubsvs.gamespy.com\r\nUser-Agent: GameSpyHTTP/1.0\r\nConnection: close\r\nContent-Length: 861\r\nContent-Type: text/xml\r\nSOAPAction: \"http://gamespy.net/AuthService/LoginUniqueNick\"\r\n\r\n<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:ns1=\"http://gamespy.net/AuthService/\"><SOAP-ENV:Body><ns1:LoginUniqueNick><ns1:version>1</ns1:version><ns1:partnercode>95</ns1:partnercode><ns1:namespaceid>95</ns1:namespaceid><ns1:uniquenick>asdf21</ns1:uniquenick><ns1:password><ns1:Value>0002899d0429a3bc5c24934320e526bed7ad645fb56c7783e2c8afef77e0dd1005d3383b58eb6919baae85881183148a4b4ebe1169475c85e305a2ca4f2d30d9ecf2275dda0706a14d15c5eb755bbca034168b13997f9111ebe821d640f64622586f231479a8516d47c046a819b19e03ba4697c68e3879dafab9380032333233</ns1:Value></ns1:password></ns1:LoginUniqueNick></SOAP-ENV:Body></SOAP-ENV:Envelope>";
            Assert.True(completeBuffer == resultreq);
            // Then
        }
    }
}