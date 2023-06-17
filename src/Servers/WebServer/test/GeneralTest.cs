using Moq;
using UniSpy.Server.Core.Abstraction.Interface;
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
    }
}