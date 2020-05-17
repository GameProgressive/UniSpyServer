using GameSpyLib.Extensions;
using NatNegotiation.Entity.Enumerator;
using System.Collections.Generic;
using System.Net;

namespace NatNegotiation.Entity.Structure.Packet
{
    public class InitPacket : BasePacket
    {
        public static new readonly int Size = BasePacket.Size + 9;

        public NatPortType PortType { get; protected set; }
        public byte ClientIndex { get; protected set; }
        public byte UseGamePort { get; protected set; }
        public string LocalIP { get; protected set; }
        public ushort LocalPort { get; protected set; }


        public override bool Parse(byte[] recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }
            PortType = (NatPortType)recv[BasePacket.Size];//
            ClientIndex = recv[BasePacket.Size + 1];//00
            UseGamePort = recv[BasePacket.Size + 2];//00

            LocalIP = HtonsExtensions.
                BytesToIPString(
                ByteTools.SubBytes(recv, BasePacket.Size + 3, 4));

            LocalPort = HtonsExtensions.
                BytesToUshortPort(
                ByteTools.SubBytes(recv, BasePacket.Size + 7, 2));

            return true;
        }

        public override byte[] BuildResponse()
        {

            List<byte> data = new List<byte>();

            data.AddRange(base.BuildResponse());

            data.Add((byte)PortType);
            data.Add(ClientIndex);
            data.Add(UseGamePort);

            data.AddRange(HtonsExtensions.IPStringToBytes(LocalIP));
            data.AddRange(HtonsExtensions.UshortPortToBytes(LocalPort));

            return data.ToArray();
        }

        public InitPacket SetIPAndPortForResponse(EndPoint end)
        {
            LocalIP = ((IPEndPoint)end).Address.ToString();
            LocalPort = (ushort)((IPEndPoint)end).Port;
            return this;
        }

        public InitPacket SetPortType(NatPortType type)
        {
            PortType = type;
            return this;
        }
    }
}
