using QueryReport.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace QueryReport.Entity.Abstraction.BaseClass
{
    public class ClientMessagePacket : BasePacket
    {

        public byte[] Message { get; protected set; }
        public int MessageKey { get; protected set; }
        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            //we need to change packet type to client message then send
            PacketType = QRPacketType.ClientMessage;
            data.AddRange(base.BuildResponse());
            data.AddRange(BitConverter.GetBytes(MessageKey));
            data.AddRange(Message);
            return data.ToArray();
        }

        public ClientMessagePacket SetMessage(byte[] message)
        {
            Message = message;
            return this;
        }

        public ClientMessagePacket SetMessageKey(int key)
        {
            MessageKey = key;
            return this;
        }
    }
}
