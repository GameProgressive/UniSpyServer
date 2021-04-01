using System.Text;

namespace UniSpyLib.Encryption
{
    public class XorEncoding
    {
        public enum XorType : uint
        {
            Type0,
            Type1,
            Type2,
            Type3
        }
        public static string Encrypt(string plainText, XorType type)
        {
            return Encoding.ASCII.GetString(
                Encrypt(
                    Encoding.ASCII.GetBytes(plainText), type));
        }
        /// <summary>
        /// simple xor encoding for Gstats,GPSP,GPCM
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name ="enc0">default encryption string used in GPSP,GPCM</param>
        /// <param name ="enc1">used in GStats</param>
        /// <param name ="enc2">used in GStats</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] plaintext, XorType type)
        {
            string seed0 = "gamespy";
            string seed1 = "GameSpy3D";
            string seed2 = "Industries";
            string seed3 = "ProjectAphex";
            //string statsfile = "gstats.dat";

            int length = plaintext.Length;
            int index = 0;
            byte[] temp;
            switch (type)
            {
                case XorType.Type0:
                    temp = Encoding.ASCII.GetBytes(seed0);
                    break;
                case XorType.Type1:
                    temp = Encoding.ASCII.GetBytes(seed1);
                    break;
                case XorType.Type2:
                    temp = Encoding.ASCII.GetBytes(seed2);
                    break;
                case XorType.Type3:
                    temp = Encoding.ASCII.GetBytes(seed3);
                    break;
                default:
                    temp = Encoding.ASCII.GetBytes(seed0);
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
    }
}
