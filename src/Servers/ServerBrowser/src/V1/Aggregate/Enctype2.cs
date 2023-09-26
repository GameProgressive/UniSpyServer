using System.Linq;
using System;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpy.Server.ServerBrowser.V1.Aggregate
{
    public class Enctype2Params
    {
        public const int CRYPT_HEADER_LENGTH = 16;
        public const int SBOX_SIZE = 256;
        public const int NUM_KEYSETUP_SKIP = 2;
        public const int CRYPT_MIN_LEAF_NUM = (1 << (CRYPT_HEADER_LENGTH));
        public uint[] Sbox = new uint[SBOX_SIZE];
        public uint[] XStack = new uint[CRYPT_HEADER_LENGTH];
        public uint[] YStack = new uint[CRYPT_HEADER_LENGTH];
        public uint[] ZStack = new uint[CRYPT_HEADER_LENGTH];
        public int Index;
        public uint X, Y, Z;
        public uint TreeNum;
        public uint[] EncryptKey { get; set; } = new uint[16];
        public byte[] Seed { get; private set; } = new byte[13];
        public bool IsInitialized = false;

        public Enctype2Params(byte[] gameSecretKey)
        {
            InitCryptParam();
            Array.Copy(gameSecretKey, Seed, gameSecretKey.Length);
        }

        private void InitCryptParam()
        {
            uint i, j, k, index;
            uint tmp;
            for (j = 0; j < 4; j++)
            {
                for (i = 0; i < Enctype2Params.SBOX_SIZE; i++)
                {
                    Sbox[i] = Sbox[i] * Enctype2Params.SBOX_SIZE + i;
                }
                index = j;
                for (k = 0; k < Enctype2Params.NUM_KEYSETUP_SKIP; k++)
                {
                    for (i = 0; i < Enctype2Params.SBOX_SIZE; i++)
                    {
                        index += (Seed[i % Seed.Length] + Sbox[i]);
                        index &= Enctype2Params.SBOX_SIZE - 1;
                        tmp = Sbox[i];
                        Sbox[i] = Sbox[index];
                        Sbox[index] = tmp;
                    }
                }
            }
            for (i = 0; i < Enctype2Params.SBOX_SIZE; i++)
            {
                Sbox[i] ^= (byte)i;
            }
            UpdateParams();
            IsInitialized = true;
        }
        public static void Accumlator(ref uint x, ref uint y, ref uint z)
        {
            x += z;
            y += x;
            x += y;
        }
        public static void ByteShift1(ref uint x, ref uint y, ref uint z, uint[] sbox)
        {
            x = ~x;
            x = (((x) << 24) | ((x) >> 8));
            x ^= sbox[x & 0xFF];
            y ^= sbox[y & 0xFF];
            x = (((x) << 24) | ((x) >> 8));
            y = (((y) << 8) | ((y) >> 24));
            x ^= sbox[x & 0xFF];
            y ^= sbox[y & 0xFF];
            y = (((y) << 8) | ((y) >> 24));
            z += (z + 1);
        }
        public static void ByteShift2(ref uint x, ref uint y, ref uint z, uint[] sbox)
        {
            y = (((y) << 24) | ((y) >> 8));
            x ^= sbox[x & 0xFF];
            y ^= sbox[y & 0xFF];
            y = (((y) << 24) | ((y) >> 8));
            x = (((x) << 8) | ((x) >> 24));
            x ^= sbox[x & 0xFF];
            y ^= sbox[y & 0xFF];
            x = (((x) << 8) | ((x) >> 24));
            z += z;
        }
        private void UpdateParams(uint treeNum = 0, uint leafNum = 0)
        {
            int i = 1 << (CRYPT_HEADER_LENGTH - 1);
            X = treeNum;
            Y = 0;
            Z = 1;
            Index = 0;
            while (i > 0)
            {
                Accumlator(ref X, ref Y, ref Z);
                if ((i & leafNum) != 0)
                {
                    ByteShift1(ref X, ref Y, ref Z, Sbox);
                }
                else
                {
                    XStack[Index] = X;
                    YStack[Index] = Y;
                    ZStack[Index] = Z;
                    Index++;

                    ByteShift2(ref X, ref Y, ref Z, Sbox);
                }
                i >>= 1;
            }
            TreeNum = treeNum;
        }
        public void InitKeyData()
        {
            uint x, y, z;
            int index;
            x = X;
            y = Y;
            z = Z;
            index = Index;
            for (uint i = 0; i < EncryptKey.Length; i++)
            {
                while (z < Enctype2Params.CRYPT_MIN_LEAF_NUM)
                {
                    Enctype2Params.Accumlator(ref x, ref y, ref z);
                    XStack[index] = x;
                    YStack[index] = y;
                    ZStack[index] = z;
                    index++;
                    Enctype2Params.ByteShift2(ref x, ref y, ref z, Sbox);
                }
                EncryptKey[i] = x ^ y;
                index--;
                if (index < 0)
                {
                    index = 0;
                }
                x = XStack[index];
                y = YStack[index];
                z = ZStack[index];
                Enctype2Params.ByteShift1(ref x, ref y, ref z, Sbox);
            }
            X = x;
            Y = y;
            Z = z;
            Index = index;
        }
    }
    public class Enctype2 : EnctypeBase, IEnctype2Test
    {
        public const int HeaderSize = 8;
        public byte[] GameSecreteKey { get; private set; }
        private Enctype2Params _params;
        private List<byte> _buffer = new List<byte>();
        public Enctype2(string gameSecretKey)
        {
            GameSecreteKey = UniSpyEncoding.GetBytes(gameSecretKey);
            _params = new Enctype2Params(GameSecreteKey);
            _buffer.Add((byte)(_params.Seed.Length ^ 0xEC));
            _buffer.AddRange(_params.Seed);
        }

        public override byte[] Encrypt(byte[] data)
        {
            int i;
            var plainText = data.ToArray();
            var encKeyBytes = EnctypeBase.ConvertUintToBytes(_params.EncryptKey);
            for (i = 0; i < plainText.Length; i++)
            {
                var modIndex = i % (encKeyBytes.Length - 1);
                if (modIndex == 0)
                {
                    _params.InitKeyData();
                    encKeyBytes = EnctypeBase.ConvertUintToBytes(_params.EncryptKey);
                }
                plainText[i] ^= encKeyBytes[modIndex];
            }
            _buffer.AddRange(plainText);
            return _buffer.ToArray();
        }


        // void IEnctype2Test.Encoder(uint[] data) => InitKeyData(data);
    }
}