using System.Linq;
using System;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Aggregate
{
    public class Enc2Params
    {
        public const int CRYPT_HEIGHT = 16;
        uint[] Sbox = new uint[256];
        uint[] XStack = new uint[CRYPT_HEIGHT];
        uint[] YStack = new uint[CRYPT_HEIGHT];
        uint[] ZStack = new uint[CRYPT_HEIGHT];
        int Index;
        uint x, y, z;
        uint TreeNum;
        uint[] KeyData = new uint[16];
    }
    public class Enctype2 : EnctypeBase, IEnctype2Test
    {
        public const int HeaderSize = 8;
        public byte[] GameSecreteKey { get; private set; }
        private Enc2Params _params = new Enc2Params();
        public Enctype2(string gameSecretKey)
        {
            GameSecreteKey = UniSpyEncoding.GetBytes(gameSecretKey);
        }

        public override byte[] Encrypt(byte[] data)
        {
            throw new System.NotImplementedException();
            //convert the byte array to unit array and parse into following functions
            // Encoder(GameSecreteKey,data,?);
        }

        private int Encoder(byte[] key, byte[] data, int size)
        {
            uint[] sbox = new uint[326];
            int i;
            // int headerSize = 8;

            for (i = size - 1; i >= 0; i--)
            {
                data[1 + HeaderSize + i] = data[i];
            }
            data[0] = (byte)HeaderSize;

            // set datap index
            byte datap = 1;
            // set 1-8 as 0
            // Array.Clear(data, datap, 8);
            // Array.Clear(sbox, 256, sbox.Length - 256);
            var sboxSeed = data.Skip(datap).ToArray();
            ConstructSbox(sboxSeed, data[0], sbox);
            Array.Copy(sboxSeed, 0, data, datap, sboxSeed.Length);
            Array.Clear(data, data[0] + size + 1, 6);

            ChangeSboxEncryptPlaintext(sbox, data, datap + data[0], size + 6);

            for (i = 0; i < key.Length; i++)
                data[datap + i] ^= key[i];
            size += 1 + data[0] + 6;
            data[0] ^= 0xec;
            return size;
        }

        private void KeyXor(byte[] data)
        {
            for (int i = 0; i < GameSecreteKey.Length; i++)
            {
                data[i] ^= GameSecreteKey[i];
            }
        }

        int IEnctype2Test.Encoder(byte[] key, byte[] data, int size) => Encoder(key, data, size);
    }
}