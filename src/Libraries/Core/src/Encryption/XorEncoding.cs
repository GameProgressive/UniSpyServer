using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Core.Encryption
{
    public enum XorType : int
    {
        Type0,
        Type1,
        Type2,
        Type3
    }
    public class XOREncoding : ICryptography
    {
        public XorType EncryptionType { get; private set; }

        public XOREncoding(XorType type)
        {
            EncryptionType = type;
        }

        public static string Encode(string plainText, XorType type) => UniSpyEncoding.GetString(Encode(UniSpyEncoding.GetBytes(plainText), type));

        /// <summary>
        /// simple xor encoding for Gstats,GPSP,GPCM
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name ="enc0">default encryption string used in GPSP,GPCM</param>
        /// <param name ="enc1">used in GStats</param>
        /// <param name ="enc2">used in GStats</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static byte[] Encode(byte[] plaintext, XorType type)
        {
            string seed0 = "gamespy";
            string seed1 = "GameSpy3D";
            string seed2 = "Industries";
            string seed3 = "ProjectAphex";

            int length = plaintext.Length;
            int index = 0;
            byte[] temp;
            switch (type)
            {
                case XorType.Type0:
                    temp = UniSpyEncoding.GetBytes(seed0);
                    break;
                case XorType.Type1:
                    temp = UniSpyEncoding.GetBytes(seed1);
                    break;
                case XorType.Type2:
                    temp = UniSpyEncoding.GetBytes(seed2);
                    break;
                case XorType.Type3:
                    temp = UniSpyEncoding.GetBytes(seed3);
                    break;
                default:
                    temp = UniSpyEncoding.GetBytes(seed0);
                    break;
            }

            for (int i = 0; length > 0; length--)
            {
                if (i >= temp.Length)
                    i = 0;

                plaintext[index++] ^= temp[i++];
            }

            return plaintext;
        }

        public byte[] Encrypt(byte[] data)
        {
            return Encode(data, EncryptionType);
        }

        public byte[] Decrypt(byte[] data)
        {
            return Encode(data, EncryptionType);
        }
    }
}
