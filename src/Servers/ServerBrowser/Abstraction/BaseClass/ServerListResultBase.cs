using ServerBrowser.Entity.Enumerate;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class ServerListResultBase : SBResultBase
    {
        public byte[] ClientRemoteIP { get; set; }
        public GameServerFlags? Flag { get; set; }
        public string GameSecretKey { get; set; }
        protected ServerListResultBase()
        {
        }
    }
}
