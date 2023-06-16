using System.Linq;
using System;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Aggregate
{

    public class Enctype2 : EnctypeBase, IEnctype2Test
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

        private int Encoder(byte[] key, byte[] data, int size)
        {
            uint[] dest = new uint[326];
            int i;
            int headerSize = 8;

            for (i = size - 1; i >= 0; i--)
            {
                data[1 + headerSize + i] = data[i];
            }
            data[0] = (byte)headerSize;

            // set datap index
            byte datap = 1;
            // set 1-8 as 0
            Array.Clear(data, datap, 8);
            Array.Clear(dest, 256, dest.Length - 256);


            var dataTemp = data.Skip(datap).ToArray();
            EncShare4(dataTemp, data[0], dest);
            Array.Copy(dataTemp, 0, data, datap, dataTemp.Length);
            Array.Clear(data, data[0] + size + 1, 6);

            Encshare1(dest, 0, data, datap + data[0], size + 6);

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