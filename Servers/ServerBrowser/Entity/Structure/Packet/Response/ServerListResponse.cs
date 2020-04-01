using System.Collections.Generic;
using System.Text;

namespace ServerBrowser.Entity.Structure.Packet.Response
{
    public class ServerListResponse
    {
        /// <summary>
        /// Combine header and crypted server data to a hole byte array
        /// </summary>
        /// <param name="data">Crypted data</param>
        /// <param name="serverChallenge"></param>
        /// <returns></returns>
        public byte[] CombineHeaderAndContext(byte[] data, string serverChallenge)
        {
            List<byte> cryptHeader = new List<byte>();

            // we add the message length here
            cryptHeader.Add(2 ^ 0xEC);
            cryptHeader.AddRange(new byte[] { 0, 0 });
            cryptHeader.Add((byte)(serverChallenge.Length ^ 0xEA));
            cryptHeader.AddRange(Encoding.ASCII.GetBytes(serverChallenge));


            cryptHeader.AddRange(data);

            return cryptHeader.ToArray();
        }
    }
}
