namespace Chat.Entity.Structure.Misc
{
    internal sealed class PeerChatCTX
    {
        public byte Buffer1;
        public byte Buffer2;
        public byte[] SBox = new byte[256];
    }
}
