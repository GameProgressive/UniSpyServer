namespace UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Misc
{
    public class StringFlag
    {
        public static readonly byte[] AllServerEndFlag = { 0, 255, 255, 255, 255 };
        public static readonly byte StringSpliter = 0;
        public static readonly byte NTSStringFlag = 0xFF;

    }
}