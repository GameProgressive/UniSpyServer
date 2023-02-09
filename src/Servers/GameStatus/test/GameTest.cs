using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using Xunit;

namespace UniSpyServer.Servers.GameStatus.Test
{
    public class GameTest
    {
        [Fact]
        public void Gmtest()
        {
            // Given
            var requests = new List<string>()
            {
                @"\auth\\gamename\crysis2\response\xxxxx\port\30\id\1",
                @"\getpd\\pid\0\ptype\0\dindex\1\keys\hello" + "\x1" + @"hi\lid\1",
                @"\getpid\\nick\xiaojiuwo\keyhash\00000\lid\1",
                @"\newgame\\connid\123\sesskey\123456\lid\1",
                @"\newgame\\connid\123\sesskey\2020\lid\1",
                @"\newgame\\connid\123\sesskey\123456\challenge\123456789\lid\1",
                @"\newgame\\connid\123\sesskey\2020\challenge\123456789\lid\1",
                @"\setpd\\pid\123\ptype\0\dindex\1\kv\%d\lid\1\length\5\data\11\lid\1",
                @"\updgame\\sesskey\0\done\1\gamedata\hello\lid\1",
                @"\updgame\\sesskey\2020\done\1\gamedata\hello\lid\1",
                @"\updgame\\sesskey\2020\connid\1\done\1\gamedata\hello\lid\1",
                @"\updgame\\sesskey\0\connid\1\done\1\gamedata\hello\lid\1",
            };
            // When
            foreach (var req in requests)
            {
                ((ITestClient)TestClasses.Client).TestReceived(UniSpyEncoding.GetBytes(req));
            }
            // Then
        }
    }
}