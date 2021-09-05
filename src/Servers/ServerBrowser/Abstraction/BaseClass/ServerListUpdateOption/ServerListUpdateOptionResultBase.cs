using ServerBrowser.Entity.Enumerate;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class ServerListUpdateOptionResultBase : SBResultBase
    {
        public byte[] ClientRemoteIP { get; set; }
        public GameServerFlags? Flag { get; set; }
        public string GameSecretKey { get; set; }
        protected ServerListUpdateOptionResultBase()
        {
        }
    }
}
