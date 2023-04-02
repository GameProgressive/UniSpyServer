using UniSpy.Server.Core.Encryption;
using UniSpy.Server.PresenceSearchPlayer.Handler;
using Xunit;

namespace UniSpy.Server.PresenceSearchPlayer.Test
{
    public class GameTest
    {
        [Fact]
        public void CheckTest()
        {
            var raw = UniSpyEncoding.GetBytes(@"\check\\nick\spyguy\email\spyguy@gamespy.com\pass\0000\final\");
            var client = TestClasses.CreateClient();
            var switcher = new CmdSwitcher(client, raw);
            switcher.Handle();
        }
    }
}