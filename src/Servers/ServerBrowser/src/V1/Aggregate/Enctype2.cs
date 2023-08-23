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
        public uint[] KeyData = new uint[16];
        public byte[] EncryptKey { get; private set; } = new byte[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public bool IsInitialized = false;

        public Enctype2Params(byte[] gameSecretKey)
        {
            Array.Copy(gameSecretKey, EncryptKey, gameSecretKey.Length);
            InitCryptParam();
        }

        private void InitCryptParam()
        {
            int i, j, k, index;
            uint tmp;
            for (j = 0; j < 4; j++)
            {
                for (i = 0; i < Sbox.Length; i++)
                {
                    Sbox[i] = (byte)(Sbox[i] * Enctype2Params.SBOX_SIZE + i);
                }
                index = j;
                for (k = 0; k < Enctype2Params.NUM_KEYSETUP_SKIP; k++)
                {
                    for (i = 0; i < Enctype2Params.SBOX_SIZE; i++)
                    {
                        index += (byte)(EncryptKey[i % EncryptKey.Length] + Sbox[i]);
                        index &= (Enctype2Params.SBOX_SIZE - 1);
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
        private static void Accumlator(ref uint x, ref uint y, ref uint z)
        {
            x += z;
            y += x;
            x += y;
        }
        private static void ByteShift1(ref uint x, ref uint y, ref uint z, uint[] sbox)
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
        private static void ByteShift2(ref uint x, ref uint y, ref uint z, uint[] sbox)
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
            int i;
            uint x, y, z;
            i = 1 << (CRYPT_HEADER_LENGTH - 1);
            x = treeNum;
            y = 0;
            z = 1;
            Index = 0;
            while (i > 0)
            {
                Accumlator(ref x, ref y, ref z);
                if ((i & leafNum) != 0)
                {
                    ByteShift1(ref x, ref y, ref z, Sbox);
                }
                else
                {
                    XStack[Index] = x;
                    YStack[Index] = y;
                    ZStack[Index] = z;
                    Index++;

                    ByteShift2(ref x, ref y, ref z, Sbox);
                }
                i >>= 1;
            }
            X = x;
            Y = y;
            Z = z;
            TreeNum = treeNum;
            // return X ^ Y;
        }
        public void RollingEncryptKey(uint startIndex = CRYPT_HEADER_LENGTH)
        {
            for (int i = 0; i < CRYPT_HEADER_LENGTH; i++)
            {
                while (Z < CRYPT_MIN_LEAF_NUM)
                {
                    Accumlator(ref X, ref Y, ref Z);
                    XStack[Index] = X;
                    YStack[Index] = Y;
                    ZStack[Index] = Z;
                    Index++;
                    ByteShift1(ref X, ref Y, ref Z, Sbox);
                }
                startIndex++;
                startIndex = X ^ Y;
                Index--;
                if (Index < 0)
                {
                    Index = 0;
                }
                X = XStack[Index];
                Y = YStack[Index];
                Z = ZStack[Index];
                ByteShift2(ref X, ref Y, ref Z, Sbox);
            }
        }
    }
    public class Enctype2 : EnctypeBase, IEnctype2Test
    {
        public const int HeaderSize = 8;
        public byte[] GameSecreteKey { get; private set; }
        private Enctype2Params _params;
        public Enctype2(string gameSecretKey)
        {
            GameSecreteKey = UniSpyEncoding.GetBytes(gameSecretKey);
            _params = new Enctype2Params(GameSecreteKey);
        }

        public override byte[] Encrypt(byte[] data)
        {
            throw new System.NotImplementedException();
            //convert the byte array to unit array and parse into following functions
            // Encoder(GameSecreteKey,data,?);
        }

        private void Encoder(byte[] key, byte[] data, int size)
        {
            int i;
            var plainText = data.ToArray();
            for (i = 0; i < plainText.Length; i++)
            {
                if (i == 0 || i >= (_params.EncryptKey.Length - 1))
                {

                }
                plainText[i] ^= _params.EncryptKey[i % _params.EncryptKey.Length];
            }
        }

        void IEnctype2Test.Encoder(byte[] key, byte[] data, int size) => Encoder(key, data, size);
    }
}