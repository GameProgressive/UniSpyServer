using GameSpyLib.Extensions;
using System;
using System.Text;

namespace ServerBrowser.Entity.Structure.Packet.Request
{
    public class PlayerSearchRequest
    {
        public byte[] SearchOption = new byte[4];
        public byte[] MaxResults = new byte[4];

        public string SearchName
        {
            get
            {
                return Encoding.ASCII.GetString(_searchName);
            }

            protected set { }
        }

        private byte[] _searchName;

        public string Message
        {
            get
            {
                return Encoding.ASCII.GetString(_message);
            }

            protected set { }
        }

        private byte[] _message;

        public PlayerSearchRequest(byte[] recv)
        {
            ByteTools.SubBytes(recv, 3, 3 + 4).CopyTo(SearchOption, 0);
            ByteTools.SubBytes(recv, 7, 7 + 4).CopyTo(MaxResults, 0);

            int nameLength = BitConverter.ToInt32(ByteTools.SubBytes(recv, 11, 11 + 4));

            _searchName = new byte[nameLength];
            ByteTools.SubBytes(recv, 15, nameLength).CopyTo(_searchName, 0);

            int messageLength = BitConverter.ToInt32(ByteTools.SubBytes(recv, 15 + nameLength, 4));

            _message = new byte[messageLength];
            ByteTools.SubBytes(recv, 15 + nameLength + 4, messageLength).CopyTo(_message, 0);
        }
    }
}
