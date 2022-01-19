using UniSpyServer.Servers.GameStatus.Entity.Enumerate;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using Xunit;

namespace UniSpyServer.Servers.GameStatus.Test
{
    public class RequestTest
    {
        [Fact]
        public void AuthPlayerTest()
        {
            var raw = @"\auth\\gamename\crysis2\response\xxxxx\port\30\id\1\final";
            var request = new AuthGameRequest(raw);
            request.Parse();
            Assert.Equal("crysis2", request.GameName);
            Assert.Equal(30, request.Port);
            Assert.Equal((uint)1, request.OperationID);
        }
        [Fact]
        public void AuthGameTest()
        {
            var raw = @"\auth\\gamename\gmtest\response\xxxx\port\0\id\1";
            var request = new AuthGameRequest(raw);
            request.Parse();
            Assert.Equal("gmtest", request.GameName);
            Assert.Equal(0, request.Port);
            Assert.Equal((uint)1, request.OperationID);
        }
        [Fact]
        public void GetPlayerDataTest()
        {
            var raw = @"\getpd\\pid\0\ptype\0\dindex\1\keys\hello" + "\x1" + @"hi\lid\1";
            var request = new GetPlayerDataRequest(raw);
            request.Parse();
            Assert.Equal((uint)0, request.ProfileID);
            Assert.Equal(PersistStorageType.PrivateRO, request.StorageType);
            Assert.Equal((uint)1, request.DataIndex);
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
            Assert.Equal((uint)1, request.OperationID);
        }
        [Fact]
        public void NewGameTest()
        {
            var raw = @"\newgame\\connid\123\sesskey\123456\lid\1";
            var request = new NewGameRequest(raw);
            request.Parse();
            Assert.Equal((uint)123, request.ConnectionID);
            Assert.Equal((uint)123456, request.SessionKey);
            Assert.Equal((uint)1, request.OperationID);

            raw = @"\newgame\\connid\123\sesskey\123456\challenge\123456789\lid\1";
            request = new NewGameRequest(raw);
            request.Parse();
            Assert.Equal((uint)123, request.ConnectionID);
            Assert.Equal((uint)123456, request.SessionKey);
            Assert.Equal("123456789", request.Challenge);
            Assert.Equal((uint)1, request.OperationID);
        }
        [Fact]
        public void SetPlayerDataTest()
        {
            var raw = @"\setpd\\pid\123\ptype\0\dindex\1\kv\%d\lid\1\length\5\data\11\lid\1";
            var request = new SetPlayerDataRequest(raw);
            request.Parse();
            Assert.Equal((uint)123, request.ProfileID);
            Assert.Equal(PersistStorageType.PrivateRO, request.StorageType);
            Assert.Equal((uint)1, request.DataIndex);
            Assert.Equal((uint)5, request.Length);

        }
        [Fact]
        public void UpdateGameTest()
        {
            var raw = @"\updgame\\sesskey\0\done\1\gamedata\hello\lid\1";
            var request = new UpdateGameRequest(raw);
            request.Parse();
            Assert.Equal((uint)0, request.SessionKey);
            Assert.Equal(true, request.IsDone);
            Assert.Equal("hello", request.GameData);
            Assert.Null(request.ConnectionID);


            raw = @"\updgame\\sesskey\0\connid\1\done\1\gamedata\hello\lid\1";
            request = new UpdateGameRequest(raw);
            request.Parse();
            Assert.Equal((uint)0, request.SessionKey);
            Assert.Equal(true, request.IsDone);
            Assert.Equal("hello", request.GameData);
            Assert.Equal((uint)1, request.ConnectionID);
        }
    }
}
