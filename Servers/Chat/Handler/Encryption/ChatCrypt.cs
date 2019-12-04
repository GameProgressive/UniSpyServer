using Chat.Structure;
using System.Text;

namespace Chat.Handler.Encryption
{
    /// <summary>
    /// This class is used to encrypt and decrypt the data for peerchat.
    /// 
    /// Note: this is a csharp version of the peerchat algorithm created by aluigi.
    /// Original C implementation: http://aluigi.altervista.org/papers/gs_peerchat.h
    /// </summary>
    public class ChatCrypt
    {

        /// <summary>
        /// Crypt or decrypt the data
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name=""></param>
        /// <param name="data"></param>
        public static void Handle(GSPeerChatCTX ctx, ref byte[] data)
        {
            byte num1 = ctx.GSPeerChat1;
            byte num2 = ctx.GSPeerChat2;
            byte[] crypt = ctx.GSPeerChatCrypt;
            byte t = 0;
            int datapos = 0;

            int size = data.Length;
            while ((size--) > 0)
            {
                t = crypt[++num1];
                num2 += t;
                crypt[num1] = crypt[num2];
                crypt[num2] = t;
                t += crypt[num1];
                data[datapos++] ^= crypt[t];
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
        public static void Init(GSPeerChatCTX ctx, string challengeString, string gameKeyString)
        {

            byte[] chall = Encoding.ASCII.GetBytes(challengeString);
            byte[] gamekey = Encoding.ASCII.GetBytes(gameKeyString);

            byte[] challenge = new byte[16];
            byte[] crypt = ctx.GSPeerChatCrypt;

            ctx.GSPeerChat1 = 0;
            ctx.GSPeerChat2 = 0;



            int gKeyi = 0;
            for (int i = 0; i < challenge.Length; i++, gKeyi++)
            {
                if (gKeyi >= gamekey.Length)
                    gKeyi = 0;

                challenge[i] = (byte)(chall[i] ^ gamekey[gKeyi]);
            }

            // Eh?
            byte t1 = 255;

            for (int i = 0; i < crypt.Length; i++, t1--)
            {
                crypt[i] = t1;
            }

            t1 = 0;

            for (int i = 0, i2 = 0; i < crypt.Length; i++, i2++)
            {
                t1 += (byte)(challenge[i2] + crypt[i]);
                byte t = crypt[t1];
                crypt[t1] = crypt[i];
                crypt[i] = t;

                if (i2 == 1)
                    i2 = 0;
            }

            ctx.GSPeerChatCrypt = crypt;
        }

        
    }
}
