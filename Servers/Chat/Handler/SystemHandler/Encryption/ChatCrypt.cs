using Chat.Entity.Structure;
using System.Text;

namespace Chat.Handler.SystemHandler.Encryption
{
    /// <summary>
    /// This class is used to encrypt and decrypt the data for peerchat.
    /// 
    /// Note: this is a csharp version of the peerchat algorithm created by aluigi.
    /// Original C implementation: http://aluigi.altervista.org/papers/gs_peerchat.h
    /// </summary>
    public class ChatCrypt
    {
        public const string DigitsHex = "0123456789abcdef";
        public const string DigitsCrypt = "aFl4uOD9sfWq1vGp";
        public const string NewDigitsCrypt = "qJ1h4N9cP3lzD0Ka";
        public const uint IPXorMask = 0xc3801dc7;


        public static void Handle(GSPeerChatCTX ctx, ref byte[] data, long size)
        {
            byte num1 = ctx.GSPeerChat1;
            byte num2 = ctx.GSPeerChat2;
            byte t;
            int datapos = 0;

            while ((size--) > 0)
            {
                num1 = (byte)((num1 + 1) % 256);
                num2 = (byte)((ctx.GSPeerChatCrypt[num1] + num2) % 256);
                t = ctx.GSPeerChatCrypt[num1];
                ctx.GSPeerChatCrypt[num1] = ctx.GSPeerChatCrypt[num2];
                ctx.GSPeerChatCrypt[num2] = t;
                t = (byte)((ctx.GSPeerChatCrypt[num2] + ctx.GSPeerChatCrypt[num1]) % 256);
                data[datapos++] ^= ctx.GSPeerChatCrypt[t];
            }

            ctx.GSPeerChat1 = num1;
            ctx.GSPeerChat2 = num2;
        }

        /// <summary>
        /// Prepare the key
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="chall"></param>
        /// <param name="gamekey"></param>
        public static void Init(GSPeerChatCTX ctx, string challengeKey, string secretKey)
        {
            byte[] challengeBytes = Encoding.ASCII.GetBytes(challengeKey);
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes(secretKey);

            ctx.GSPeerChat1 = 0;
            ctx.GSPeerChat2 = 0;

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
                ctx.GSPeerChatCrypt[i] = index1;
            }

            index1 = 0;

            for (int i = 0, index2 = 0; i < ctx.GSPeerChatCrypt.Length; i++, index1++)
            {
                if (index1 >= challengeBytes.Length)
                {
                    index1 = 0;
                }

                index2 = (byte)((challengeBytes[index1] + ctx.GSPeerChatCrypt[i] + index2) % 256);
                byte t = ctx.GSPeerChatCrypt[i];
                ctx.GSPeerChatCrypt[i] = ctx.GSPeerChatCrypt[index2];
                ctx.GSPeerChatCrypt[index2] = t;
            }
        }


        public static bool EncodeIP()
        {
            return false;
        }
    }
}
