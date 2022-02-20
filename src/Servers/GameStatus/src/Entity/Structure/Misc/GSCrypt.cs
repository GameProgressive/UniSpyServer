using System;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Misc
{
    public class GSCrypt : ICryptography
    {
        /// <summary>
        /// Decrypt the message, skip decrypt \final\
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] buffer)
        {
            //remove \final\, later we add final back
            byte[] cipher = buffer.Take(buffer.Length - 7).ToArray();
            byte[] plain = XOREncoding.Encode(cipher, XorType.Type1);
            Array.Copy(plain, buffer, plain.Length);
            return buffer;
        }
        /// <summary>
        /// Encrypt the message, skip encrypt \final\
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] buffer)
        {
            byte[] plain = buffer.Take(buffer.Length - 7).ToArray();
            byte[] cipher = XOREncoding.Encode(plain, XorType.Type1);
            Array.Copy(cipher, buffer, cipher.Length);
            return buffer;
        }
    }
}
