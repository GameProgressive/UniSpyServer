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
            string enc0 = "gamespy";
            string enc1 = "GameSpy3D";// '\0','a','m','e','S','p','y','3','D','\0'
            string enc2 = "Industries";// { '\0', 'n', 'd', 'u', 's', 't', 'r', 'i', 'e', 's', '\0' }
            string enc3 = "ProjectAphex";// { '\0','r','o','j','e','c','t','A','p','h','e','x','\0'}
            //string statsfile = "gstats.dat";

            int length = plaintext.Length;
            int index = 0;
            byte[] temp;
            switch (type)
            {
                case XorType.Type0:
                    temp = Encoding.ASCII.GetBytes(enc0);
                    break;
                case XorType.Type1:
                    temp = Encoding.ASCII.GetBytes(enc1);
                    break;
                case XorType.Type2:
                    temp = Encoding.ASCII.GetBytes(enc2);
                    break;
                case XorType.Type3:
                    temp = Encoding.ASCII.GetBytes(enc3);
                    break;
                default:
                    temp = Encoding.ASCII.GetBytes(enc0);
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
