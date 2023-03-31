using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using Xunit;

namespace UniSpy.Server.GameStatus.Test
{
    public class GameTest
    {
        [Fact]
        public void Worm3d20230331()
        {
            var client1 = (ITestClient)TestClasses.CreateClient(port: 123);
            var client2 = (ITestClient)TestClasses.CreateClient(port: 124);
            // Given
            var requests = new List<KeyValuePair<ITestClient, string>>
            {
                new KeyValuePair<ITestClient,string>(client1,@"\auth\\gamename\worms3\response\bc3ca727a7825879eb9f13d9fd51bbb9\port\0\id\1\final\"),
                new KeyValuePair<ITestClient, string>(client2,@"\auth\\gamename\worms3\response\bc3ca727a7825879eb9f13d9fd51bbb9\port\0\id\1\final\"),
                new KeyValuePair<ITestClient, string>(client2,@"\newgame\\connid\0\sesskey\144562\final\"),
                new KeyValuePair<ITestClient, string>(client2,@"\newgame\\connid\0\sesskey\144563\final\"),
                new KeyValuePair<ITestClient, string>(client1,@"\authp\\pid\1\resp\7b6658e99f448388fbeddc93654e6dd4\lid\2\final\"),
                new KeyValuePair<ITestClient, string>(client1,@"\authp\\pid\1\resp\7b6658e99f448388fbeddc93654e6dd4\lid\1\final\"),
                new KeyValuePair<ITestClient, string>(client1,@"\setpd\\pid\1\ptype\1\dindex\0\kv\1\lid\2\length\111\data\\report\|title||victories|0|timestamp|66613|league|Team17|winner||crc|-1|player_0|spyguy|ip_0||pid_0|0|auth_0|[00]\final\"),
                new KeyValuePair<ITestClient, string>(client2,@"\authp\\pid\16\resp\7b6658e99f448388fbeddc93654e6dd4\lid\2\final\"),
                new KeyValuePair<ITestClient, string>(client2,@"\authp\\pid\16\resp\7b6658e99f448388fbeddc93654e6dd4\lid\1\final\"),
                new KeyValuePair<ITestClient, string>(client2,@"\setpd\\pid\16\ptype\1\dindex\0\kv\1\lid\2\length\125\data\\report\|title||victories|0|timestamp|66613|league|Team17|winner||crc|-1|player_0|unispy|ip_0|192.168.1.102|pid_0|16|auth_0|[00]\final\")
        };

            foreach (var item in requests)
            {
                item.Key.TestReceived(UniSpyEncoding.GetBytes(item.Value));
            }
        }
        [Fact]
        public void Gmtest()
        {
            // Given
            var requests = new List<string>()
            {
                @"\auth\\gamename\crysis2\response\xxxxx\port\30\id\1\final\",
                @"\getpd\\pid\0\ptype\0\dindex\1\keys\hello" + "\x1" + @"hi\lid\1\final\",
                @"\getpid\\nick\xiaojiuwo\keyhash\00000\lid\1\final\",
                @"\newgame\\connid\123\sesskey\123456\lid\1\final\",
                @"\newgame\\connid\123\sesskey\2020\lid\1\final\",
                @"\newgame\\connid\123\sesskey\123456\challenge\123456789\lid\1\final\",
                @"\newgame\\connid\123\sesskey\2020\challenge\123456789\lid\1\final\",
                @"\setpd\\pid\123\ptype\0\dindex\1\kv\%d\lid\1\length\5\data\11\lid\1\final\",
                @"\updgame\\sesskey\0\done\1\gamedata\hello\lid\1\final\",
                @"\updgame\\sesskey\2020\done\1\gamedata\hello\lid\1\final\",
                @"\updgame\\sesskey\2020\connid\1\done\1\gamedata\hello\lid\1\final\",
                @"\updgame\\sesskey\0\connid\1\done\1\gamedata\hello\lid\1\final\",
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