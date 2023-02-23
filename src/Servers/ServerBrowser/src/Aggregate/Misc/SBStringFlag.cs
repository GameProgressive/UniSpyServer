namespace UniSpy.Server.ServerBrowser.Aggregate.Misc
{
    public class StringFlag
    {
        public static readonly byte[] AllServerEndFlag = { 0, 255, 255, 255, 255 };
        public static readonly byte StringSpliter = 0;
        public static readonly byte NTSStringFlag = 0xFF;

    }
}
