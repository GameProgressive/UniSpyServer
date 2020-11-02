using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.Request
{
    public class ClientMessageResponse:QRResponseBase
    {

        public byte[] Message { get; protected set; }
        public int MessageKey { get; protected set; }

        public ClientMessageResponse(byte[] message,int messageKey,QRRequestBase request) : base(request)
        {
            Message = message;
            MessageKey = messageKey;
            PacketType = QRPacketType.ClientMessage;
        }

        public ClientMessageResponse(byte[] message, int messageKey, int instantKey) : base(QRPacketType.ClientMessage, instantKey)
        {
            Message = message;
            MessageKey = messageKey;
        }

        public override byte[] BuildResponse()
        {
            List<byte> data = new List<byte>();
            //we need to change packet type to client message then send
            //PacketType = QRPacketType.ClientMessage;
            data.AddRange(base.BuildResponse());
            data.AddRange(BitConverter.GetBytes(MessageKey));
            data.AddRange(Message);
            return data.ToArray();
        }

        public ClientMessageResponse SetMessage(byte[] message)
        {
            Message = message;
            return this;
        }

        public ClientMessageResponse SetMessageKey(int key)
        {
            MessageKey = key;
            return this;
        }
    }
}
