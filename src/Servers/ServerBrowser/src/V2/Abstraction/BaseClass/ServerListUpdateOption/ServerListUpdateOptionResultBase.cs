using UniSpy.Server.ServerBrowser.V2.Enumerate;

namespace UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass
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
