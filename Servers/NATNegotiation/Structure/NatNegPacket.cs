using System;
using GameSpyLib.Extensions;
using NATNegotiation.Enumerator;
using NATNegotiation.Structure;

namespace RetroSpyServer.Servers.NatNeg.Structures
{

    public class NatNegPacket
    {
        //public PreinitPacket Preinit;
        public CommonInfo Common = new CommonInfo();
        public InitPacket Init = new InitPacket();
        public ConnectPacket Connect = new ConnectPacket();
        public ReportPacket Report = new ReportPacket();
        public PreinitPacket PreInit = new PreinitPacket();

        public int magicDataLen = NatNegInfo.MagicData.Length;

        public bool SetData(byte[] data)
        {
            if (data.Length < CommonInfo.Size)
                return false;

            byte[] sub = ByteExtensions.SubBytes(data, 0, magicDataLen);
            //bool isEqual = Array.Equals(sub, NatNegInfo.MagicData);
            if (sub[0] != NatNegInfo.MagicData[0] || sub[1] != NatNegInfo.MagicData[1] || sub[2] != NatNegInfo.MagicData[2])
                return false;

            if (data[magicDataLen + 2] < 0 || data[magicDataLen + 2] > (byte)NatPacketType.PreInitAck)
                return false;

            Common.Version = data[magicDataLen];
            Common.PacketType = (NatPacketType)data[magicDataLen + 1];
            Common.Cookie = BitConverter.ToInt32(ByteExtensions.SubBytes(data, magicDataLen + 2, 4));

            switch (Common.PacketType)
            {
                case NatPacketType.PreInit:
                case NatPacketType.PreInitAck:
                    SetPreInitData(data);
                    break;
                case NatPacketType.AddressReply:
                case NatPacketType.NatifyRequest:
                case NatPacketType.ErtTest:
                case NatPacketType.Init:
                case NatPacketType.InitAck:
                    SetInitData(data);
                    break;
                case NatPacketType.ConnectAck:
                case NatPacketType.ConnectPing:
                case NatPacketType.Connect:
                    SetConnectData(data);
                    break;
                case NatPacketType.Report:
                case NatPacketType.ReportAck:
                    SetReportData(data);
                    break;
                default:
                    break;
            }


            return true;
        }
        #region Store information in UdpPacket into NatNegPacket
        private void SetPreInitData(byte[] data)
        {

        }
        private void SetInitData(byte[] data)
        {
            Init.PortType = data[13];//02
            Init.ClientIndex = data[14];//00
            Init.UseGamePort = data[15];//00
            Init.LocalIp = BitConverter.ToUInt32(ByteExtensions.SubBytes(data, 16, sizeof(uint)));//00 - 00 - 00 - 00
            Init.LocalPort = BitConverter.ToUInt16(ByteExtensions.SubBytes(data, 20, sizeof(ushort)));//00 - 00
        }
        private void SetConnectData(byte[] data)
        {
            Connect.RemoteIP = BitConverter.ToUInt32(ByteExtensions.SubBytes(data, 13, sizeof(uint)));
            Connect.RemotePort = BitConverter.ToUInt16(ByteExtensions.SubBytes(data, 17, sizeof(ushort)));
            Connect.GotYourData = data[19];
            Connect.Finished = data[20];
        }
        private void SetReportData(byte[] data)
        {
            Report.PortType = data[13];
            Report.ClientIndex = data[14];
            Report.NegResult = data[15];

            byte[] tempNatType = ByteExtensions.SubBytes(data, 17, sizeof(int));
            Report.NatType = (NatType)BitConverter.ToInt32(tempNatType, 0);

            byte[] tempNatMappingScheme = ByteExtensions.SubBytes(data, 19, sizeof(int));
            Report.NatMappingScheme = (NatMappingScheme)BitConverter.ToInt32(tempNatMappingScheme, 0);

            Array.Copy(data, 23, Report.GameName, 0, 50);
        }
        #endregion
        /// <summary>
        /// Get repsonse packet size from natneg recieved packet type
        /// </summary>
        /// <param name="type">recieved packet type</param>
        /// <returns></returns>
        public int GetReplyPacketSize()
        {
            //The size is initially CommonInfo size
            int size = CommonInfo.Size;

            switch (Common.PacketType)
            {
                case NatPacketType.PreInit:
                case NatPacketType.PreInitAck:
                    size += PreinitPacket.PacketSize;
                    break;
                case NatPacketType.AddressCheck:
                case NatPacketType.NatifyRequest:
                case NatPacketType.ErtTest:
                case NatPacketType.ErtAck:
                case NatPacketType.Init:
                case NatPacketType.InitAck:
                case NatPacketType.ConnectAck:
                case NatPacketType.ReportAck:
                    size += InitPacket.PacketSize;
                    break;

                case NatPacketType.ConnectPing:
                case NatPacketType.Connect:
                    size += ConnectPacket.PacketSize;
                    break;
                case NatPacketType.Report:
                    size += ReportPacket.PacketSize;
                    break;
                default:
                    break;
            }

            return size;
        }

    }


    #region Other Packet class
    public class CommonInfo
    {
        public byte Version;
        public NatPacketType PacketType;
        public int Cookie;

        public static int Size = NatNegInfo.MagicData.Length + 6;
    }

    public class PreinitPacket
    {
        public byte ClientIndex;
        public byte State;
        public int ClientID;

        public static int PacketSize = 6;
    }

    public class InitPacket
    {
        public byte PortType;
        public byte ClientIndex;
        public byte UseGamePort;
        public uint LocalIp;
        public ushort LocalPort;

        public static int PacketSize = 9;
    }

    public class ReportPacket
    {
        public byte PortType;
        public byte ClientIndex;
        public byte NegResult;
        public NatType NatType; //int
        public NatMappingScheme NatMappingScheme; //int
        public byte[] GameName = new byte[50];

        public static int PacketSize = 61;
    }

    public class ConnectPacket
    {
        public uint RemoteIP;
        public ushort RemotePort;
        public byte GotYourData;
        public byte Finished;

        public static int PacketSize = 8;
    }
}
#endregion