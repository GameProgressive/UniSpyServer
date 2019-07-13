using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures
{
    public static class NNMagicData
    {
        public const int NATNEG_MAGIC_LEN = 6;

        public const byte NN_MAGIC_0 = 0xFD;

        public const byte NN_MAGIC_1 = 0xFC;

        public const byte NN_MAGIC_2 = 0x1E;

        public const byte NN_MAGIC_3 = 0x66;

        public const byte NN_MAGIC_4 = 0x6A;

        public const byte NN_MAGIC_5 = 0xB2;

        public static readonly byte[] MagicData = { NN_MAGIC_0, NN_MAGIC_1, NN_MAGIC_2, NN_MAGIC_3, NN_MAGIC_4, NN_MAGIC_5 };
    }
}
