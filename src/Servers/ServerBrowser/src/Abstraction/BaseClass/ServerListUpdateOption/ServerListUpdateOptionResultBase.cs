using UniSpy.Server.ServerBrowser.Enumerate;

namespace UniSpy.Server.ServerBrowser.Abstraction.BaseClass
{
    public abstract class ServerListUpdateOptionResultBase : ResultBase
    {
        public byte[] ClientRemoteIP { get; set; }
        public GameServerFlags Flag { get; set; }
        public string GameSecretKey { get; set; }
        protected ServerListUpdateOptionResultBase()
        {
        }
    }
}
