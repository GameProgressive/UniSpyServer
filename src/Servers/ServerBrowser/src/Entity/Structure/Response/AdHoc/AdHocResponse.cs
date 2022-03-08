using System;
using System.Collections.Generic;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Packet.Response
{
    public class AdHocResponse
    {
        byte[] _keyValueData;

        public AdHocResponse(byte[] keyValue)
        {
            _keyValueData = keyValue;
        }

        public byte[] GenerateByteArray()
        {
            //the 2 bytes are length of this request
            byte[] byteLength = BitConverter.GetBytes((ushort)(_keyValueData.Length + 2));
            List<byte> data = new List<byte>();
            data.AddRange(byteLength);
            data.AddRange(_keyValueData);

            return data.ToArray();
        }
    }
}
