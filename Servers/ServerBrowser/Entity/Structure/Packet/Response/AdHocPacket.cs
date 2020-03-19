using System;
using System.Collections.Generic;

namespace ServerBrowser.Entity.Structure.Packet.Response
{
    public class AdHocPacket
    {
        byte[] _keyValueData;

        public AdHocPacket(byte[] keyValue)
        {
            _keyValueData = keyValue;
        }

        public byte[] GenerateByteArray()
        {
            //the 2 bytes are length of this request
            byte[] length = new byte[2];

            length = BitConverter.GetBytes((ushort)(_keyValueData.Length + 2));
            List<byte> data = new List<byte>();
            data.AddRange(length);
            data.AddRange(_keyValueData);

            return data.ToArray();
        }
    }
}
