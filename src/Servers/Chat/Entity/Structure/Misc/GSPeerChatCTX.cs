﻿namespace Chat.Entity.Structure.Misc
{
    internal sealed class CipherText
    {
        public byte GSPeerChat1;
        public byte GSPeerChat2;
        public byte[] GSPeerChatCrypt = new byte[256];
    }
}
