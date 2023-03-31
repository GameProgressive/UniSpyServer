using UniSpy.Server.GameStatus.Enumerate;
using UniSpy.Server.GameStatus.Contract.Request;
using Xunit;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.GameStatus.Test
{
    public class RequestTest
    {
        [Fact]
        public void SetPlayerData20230329()
        {
            // Given
            var raw = @"\setpd\\pid\1\ptype\1\dindex\0\kv\1\lid\2\length\111\data\\report\|title||victories|0|timestamp|37155|league|Team17|winner||crc|-1|player_0|spyguy|ip_0||pid_0|0|auth_0|[00]\final\";
            var req = new SetPlayerDataRequest(raw);
            req.Parse();
            // When

            // Then
        }
        [Fact]
        public void GameSpySDKUpdateGameRequest20230329()
        {
            var raw1 = "\\updgame\\\\sesskey\\20298203\\connid\\0\\done\\0\\gamedata\\\u0001hostname\u0001My l33t Server\u0001mapname\u0001Level 33\u0001gametype\u0001hunter\u0001gamever\u00011.230000\u0001player_0\u0001Bob!\u0001points_0\u00014\u0001deaths_0\u00012\u0001pid_0\u000132432423\u0001auth_0\u00017cca8e60a13781eebc820a50754f57cd\u0001player_1\u0001Joey\u0001points_1\u00012\u0001deaths_1\u00014\u0001pid_1\u0001643423\u0001auth_1\u000119ea14d9d92a7fcc635cf5716944d9bc\\final\\";
            var raw2 = "\\updgame\\\\sesskey\\20298203\\connid\\0\\done\\1\\gamedata\\\u0001hostname\u0001My l33t Server\u0001mapname\u0001Level 33\u0001gametype\u0001hunter\u0001gamever\u00011.230000\u0001player_0\u0001Bob!\u0001points_0\u00016\u0001deaths_0\u00013\u0001pid_0\u000132432423\u0001auth_0\u00017cca8e60a13781eebc820a50754f57cd\u0001player_1\u0001Joey\u0001points_1\u00013\u0001deaths_1\u00016\u0001pid_1\u0001643423\u0001auth_1\u000119ea14d9d92a7fcc635cf5716944d9bc\\final\\";
            var r1 = new UpdateGameRequest(raw1);
            r1.Parse();
            var r2 = new UpdateGameRequest(raw2);
            r2.Parse();
        }
        [Fact]
        public void Worm3dAuthPlayer()
        {
            var request = "2\x0F\x16\x10]%+=veKaB3a(UC`b$\x1CO\x11VZX\x09w\x1Cu\x08L@\x13=X!\x1E{\x0EL\x1DLf[qN \x04G\x130[#N'\x09(IC`b$";
            var buffer = UniSpyEncoding.GetBytes(request);
            var buffer2 = XOREncoding.Encode(buffer, XorType.Type1);
            var data = UniSpyEncoding.GetString(buffer2);
        }
        [Fact]
        public void AuthPlayerTest()
        {
            var raw = @"\auth\\gamename\crysis2\response\xxxxx\port\30\id\1\final";
            var request = new AuthGameRequest(raw);
            request.Parse();
            Assert.Equal("crysis2", request.GameName);
            Assert.Equal(30, request.Port);
            Assert.Equal((int)1, request.LocalId);
        }
        [Fact]
        public void AuthGameTest()
        {
            var raw = @"\auth\\gamename\gmtest\response\xxxx\port\0\id\1";
            var request = new AuthGameRequest(raw);
            request.Parse();
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal(0, request.Port);
            Assert.Equal((int)1, request.LocalId);
        }
        [Fact]
        public void GetPlayerDataTest()
        {
            var raw = @"\getpd\\pid\0\ptype\0\dindex\1\keys\hello" + "\x1" + @"hi\lid\1";
            var request = new GetPlayerDataRequest(raw);
            request.Parse();
            Assert.Equal((int)0, request.ProfileId);
            Assert.Equal(PersistStorageType.PrivateRO, request.StorageType);
            Assert.Equal((int)1, request.DataIndex);
            Assert.Equal(2, request.Keys.Count);
            Assert.Equal("hello", request.Keys[0]);
            Assert.Equal("hi", request.Keys[1]);
        }
        [Fact]
        public void GetProfileIDTest()
        {
            var raw = @"\getpid\\nick\xiaojiuwo\keyhash\00000\lid\1";
            var request = new GetProfileIDRequest(raw);
            request.Parse();
            Assert.Equal("xiaojiuwo", request.Nick);
            Assert.Equal("00000", request.KeyHash);
            Assert.Equal((int)1, request.LocalId);
        }
        [Fact]
        public void NewGameTest()
        {
            var raw = @"\newgame\\connid\123\sesskey\123456\lid\1";
            var request = new NewGameRequest(raw);
            request.Parse();
            Assert.Equal((int)123, request.ConnectionID);
            Assert.Equal((int)123456, request.SessionKey);
            Assert.Equal((int)1, request.LocalId);

            raw = @"\newgame\\connid\123\sesskey\123456\challenge\123456789\lid\1";
            request = new NewGameRequest(raw);
            request.Parse();
            Assert.Equal((int)123, request.ConnectionID);
            Assert.Equal((int)123456, request.SessionKey);
            Assert.Equal("123456789", request.Challenge);
            Assert.Equal((int)1, request.LocalId);
        }
        [Fact]
        public void SetPlayerDataTest()
        {
            var raw = @"\setpd\\pid\123\ptype\0\dindex\1\kv\%d\lid\1\length\5\data\11\lid\1";
            var request = new SetPlayerDataRequest(raw);
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => request.Parse());
            raw = @"\setpd\\pid\123\ptype\0\dindex\1\kv\%d\lid\1\report\hello\length\5\data\11\lid\1";
            request = new SetPlayerDataRequest(raw);
            request.Parse();
            Assert.Equal((int)123, request.ProfileId);
            Assert.Equal(PersistStorageType.PrivateRO, request.StorageType);
            Assert.Equal((int)1, request.DataIndex);
            Assert.Equal((int)5, request.Length);

        }
        [Fact]
        public void UpdateGameTest()
        {
            var raw = @"\updgame\\sesskey\0\done\1\gamedata\hello\lid\1";
            var request = new UpdateGameRequest(raw);
            request.Parse();
            Assert.True(0 == request.SessionKey);
            Assert.True(true == request.IsDone);
            Assert.True("hello" == request.GameData);
            Assert.Null(request.ConnectionID);


            raw = @"\updgame\\sesskey\0\connid\1\done\1\gamedata\hello\lid\1";
            request = new UpdateGameRequest(raw);
            request.Parse();
            Assert.True(0 == request.SessionKey);
            Assert.True(true == request.IsDone);
            Assert.True("hello" == request.GameData);
            Assert.True(1 == request.ConnectionID);
        }
    }
}
