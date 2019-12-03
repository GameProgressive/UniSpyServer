using Chat.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Encryption
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
        /// Prepare the key
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="chall"></param>
        /// <param name="gamekey"></param>
        public static void Init(ref GSPeerChatCTX ctx, byte[] chall, byte[] gamekey)
        {
            byte[] challenge = new byte[16];
            byte[] crypt = null;

            ctx.GSPeerChat1 = 0;
            ctx.GSPeerChat2 = 0;

            crypt = ctx.GSPeerChatCrypt;

            int gkeyi = 0;
            for (int i = 0; i < challenge.Length; i++, gkeyi++)
            {
                if (gkeyi >= gamekey.Length)
                    gkeyi = 0;

                challenge[i] = (byte)(chall[i] ^ gamekey[gkeyi]);
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

        /// <summary>
        /// Crypt or decrypt the data
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name=""></param>
        /// <param name="data"></param>
        public static void Handle(ref GSPeerChatCTX ctx, ref byte[] data)
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
    }
}
