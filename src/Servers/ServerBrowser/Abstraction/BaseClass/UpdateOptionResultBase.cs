namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class UpdateOptionResultBase : SBResultBase
    {
        public byte[] ClientRemoteIP { get; set; }
        protected UpdateOptionResultBase()
        {
        }
    }
}
