using System;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Aggregate
{

    public class Enctype2 : EnctypeBase
    {
        public const int HeaderSize = 8;
        public byte[] GameSecreteKey { get; private set; }
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

        public int Encoder(byte[] key, byte[] data, int size)
        {
            uint[] dest = new uint[326];
            int i;
            int headerSize = 8;

            for (i = size - 1; i >= 0; i--)
            {
                data[1 + headerSize + i] = data[i];
            }
            data[0] = (byte)headerSize;

            byte[] datap = new byte[data.Length - 1];
            Array.Copy(data, 1, datap, 0, datap.Length);
            Array.Clear(datap, 0, data[0]);

            for (i = 256; i < 326; i++)
                dest[i] = 0;
            EncShare4(datap, data[0], dest);

            Array.Clear(data, 1 + data[0] + size, 6);
            Encshare1(dest, datap, size + 6);

            for (i = 0; i < key.Length; i++)
                datap[i] ^= key[i];
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
    }
}