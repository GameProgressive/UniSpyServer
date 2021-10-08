using System.Collections.Generic;
using UniSpyLib.Encryption;

namespace Chat.Entity.Structure.Misc
{
    /// <summary>
    /// This class is used to encrypt and decrypt the data for peerchat.
    /// 
    /// Note: this is a csharp version of the peerchat algorithm created by aluigi.
    /// Original C implementation: http://aluigi.altervista.org/papers/gs_peerchat.h
    /// </summary>
    internal sealed class ChatCrypt
    {
        public const string DigitsHex = "0123456789abcdef";
        public const string DigitsCrypt = "aFl4uOD9sfWq1vGp";
        public const string NewDigitsCrypt = "qJ1h4N9cP3lzD0Ka";
        public const uint IPXorMask = 0xc3801dc7;

        public static byte[] Handle(PeerChatCTX ctx, byte[] data)
        {
            byte num1 = ctx.Buffer1;
            byte num2 = ctx.Buffer2;
            byte t;
            int datapos = 0;
            List<byte> buffer = new List<byte>();
            long size = data.Length;
            while (size-- > 0)
            {
                num1 = (byte)((num1 + 1) % 256);
                num2 = (byte)((ctx.SBox[num1] + num2) % 256);
                t = ctx.SBox[num1];
                ctx.SBox[num1] = ctx.SBox[num2];
                ctx.SBox[num2] = t;
                t = (byte)((ctx.SBox[num2] + ctx.SBox[num1]) % 256);
                data[datapos++] ^= ctx.SBox[t];
                byte temp = (byte)(data[datapos++] ^ ctx.SBox[t]);
                buffer.Add(temp);
            }

            ctx.Buffer1 = num1;
            ctx.Buffer2 = num2;
            return buffer.ToArray();
        }

        /// <summary>
        /// Prepare the key
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="chall"></param>
        /// <param name="gamekey"></param>
        public static void Init(PeerChatCTX ctx, string challengeKey, string secretKey)
        {
            byte[] challengeBytes = UniSpyEncoding.GetBytes(challengeKey);
            byte[] secretKeyBytes = UniSpyEncoding.GetBytes(secretKey);

            ctx.Buffer1 = 0;
            ctx.Buffer2 = 0;

            int secretKeyIndex = 0;
            for (int i = 0; i < challengeBytes.Length; i++, secretKeyIndex++)
            {
                if (secretKeyIndex >= secretKeyBytes.Length)
                {
                    secretKeyIndex = 0;
                }

                challengeBytes[i] ^= secretKeyBytes[secretKeyIndex];
            }

            byte index1 = 255;

            for (int i = 0; i < 256; i++, index1--)
            {
                ctx.SBox[i] = index1;
            }

            index1 = 0;

            for (int i = 0, index2 = 0; i < ctx.SBox.Length; i++, index1++)
            {
                if (index1 >= challengeBytes.Length)
                {
                    index1 = 0;
                }

                index2 = (byte)((challengeBytes[index1] + ctx.SBox[i] + index2) % 256);
                byte t = ctx.SBox[i];
                ctx.SBox[i] = ctx.SBox[index2];
                ctx.SBox[index2] = t;
            }
        }


        public static bool EncodeIP()
        {
            return false;
        }
    }
}
