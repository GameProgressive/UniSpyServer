using RetroSpyServer.Servers.NatNeg.Enumerators;
using System;
using GameSpyLib.Extensions;

namespace RetroSpyServer.Servers.NatNeg.Structures
{

    public class NatNegPacket
    {
        //public PreinitPacket Preinit;
        public CommonPacket Common = new CommonPacket();
        public InitPacket Init = new InitPacket();
        public ConnectPacket Connect = new ConnectPacket();
        public ReportPacket Report = new ReportPacket();
        public PreinitPacket PreInit = new PreinitPacket();

        private int magicDataLen = NatNegInfo.MagicData.Length;

        public bool Set(byte[] data)
        {
            if (data.Length < CommonPacket.Size())
                return false;

            if (!ByteExtensions.SubBytes(data, 0, magicDataLen).Equals(NatNegInfo.MagicData))
                return false;

            if (data[magicDataLen + 2] < 0 || data[magicDataLen + 2] > (byte)NatPacketType.PreInitAck)
                return false;

            Common.Version = data[magicDataLen + 1];
            Common.PacketType = (NatPacketType)data[magicDataLen + 2];
            Common.Cookie = BitConverter.ToInt32(ByteExtensions.SubBytes(data, magicDataLen + 3, 4));

            switch (Common.PacketType)
            {
                case NatPacketType.PreInit:
                case NatPacketType.PreInitAck:
                    PreInitToBytes(data);
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

        public byte[] ToBytes()
        {
            byte[] returnBytes = new byte[GetPacketSize()];
            NatNegInfo.MagicData.CopyTo(returnBytes, 0);
        
            returnBytes[magicDataLen] = Common.Version;
            returnBytes[magicDataLen + 1] = (byte)Common.PacketType;
            BitConverter.GetBytes(Common.Cookie).CopyTo(returnBytes, magicDataLen + 2);

            switch (Common.PacketType)
            {
                case NatPacketType.PreInit:
                case NatPacketType.PreInitAck:
                    PreInitToBytes(returnBytes);
                    break;
                case NatPacketType.AddressReply:
                case NatPacketType.NatifyRequest:
                case NatPacketType.ErtTest:
                case NatPacketType.Init:
                case NatPacketType.InitAck:
                    InitToBytes(returnBytes);
                    break;
                case NatPacketType.ConnectAck:
                case NatPacketType.ConnectPing:
                case NatPacketType.Connect:
                    ConnectToBytes(returnBytes);
                    break;
                case NatPacketType.Report:
                case NatPacketType.ReportAck:
                    ReportToBytes(returnBytes);
                    break;
                default:
                    break;
            }

            return returnBytes;
        }

        private void PreInitToBytes(byte[] returnBytes)
        {
            returnBytes[CommonPacket.Size()] = PreInit.ClientIndex;
            returnBytes[CommonPacket.Size() + 1] = PreInit.State;
            BitConverter.GetBytes(PreInit.State).CopyTo(returnBytes, CommonPacket.Size() + 2);
        }

        private void InitToBytes(byte[] returnBytes)
        {
            returnBytes[CommonPacket.Size()] = Init.PortType;
            returnBytes[CommonPacket.Size() + 1] = Init.ClientIndex;
            returnBytes[CommonPacket.Size() + 2] = Init.UseGamePort;
            BitConverter.GetBytes(Init.LocalIp).CopyTo(returnBytes, CommonPacket.Size() + 3);
            BitConverter.GetBytes(Init.LocalPort).CopyTo(returnBytes, CommonPacket.Size() + 7);
        }

        private void ConnectToBytes(byte[] returnBytes)
        {
            BitConverter.GetBytes(Connect.RemoteIP).CopyTo(returnBytes, CommonPacket.Size());
            BitConverter.GetBytes(Connect.RemotePort).CopyTo(returnBytes, CommonPacket.Size() + 4);

            returnBytes[CommonPacket.Size()] = Connect.GotYourData;
            returnBytes[CommonPacket.Size() + 1] = Connect.Finished;
        }

        private void ReportToBytes(byte[] returnBytes)
        {
            returnBytes[CommonPacket.Size()] = Report.PortType;
            returnBytes[CommonPacket.Size() + 1] = Report.ClientIndex;
            returnBytes[CommonPacket.Size() + 2] = Report.NegResult;
            BitConverter.GetBytes((int)Report.NatType).CopyTo(returnBytes, CommonPacket.Size() + 3);
            BitConverter.GetBytes((int)Report.NatMappingScheme).CopyTo(returnBytes, CommonPacket.Size() + 7);
            Report.GameName.CopyTo(returnBytes, CommonPacket.Size() + 11);
        }

        /// <summary>
        /// Get repsonse packet size from natneg recieved packet type
        /// </summary>
        /// <param name="type">recieved packet type</param>
        /// <returns></returns>
        public int GetPacketSize()
        {
            int size = CommonPacket.Size();
            switch (Common.PacketType)
            {
                case NatPacketType.PreInit:
                case NatPacketType.PreInitAck:
                    size += PreinitPacket.Size();
                    break;
                case NatPacketType.AddressCheck:
                case NatPacketType.NatifyRequest:
                case NatPacketType.ErtTest:
                case NatPacketType.ErtAck:
                case NatPacketType.Init:
                case NatPacketType.InitAck:
                case NatPacketType.ConnectAck:
                case NatPacketType.ReportAck:
                    size += InitPacket.Size();
                    break;

                case NatPacketType.ConnectPing:
                case NatPacketType.Connect:
                    size += ConnectPacket.Size();
                    break;
                case NatPacketType.Report:
                    size += ReportPacket.Size();
                    break;
                default:
                    break;
            }

            return size;
        }

    }

    public class CommonPacket
    {
        public byte Version;
        public NatPacketType PacketType;
        public int Cookie;

        public static int Size() { return NatNegInfo.MagicData.Length + 6; }
    }

    public class PreinitPacket
    {
        public byte ClientIndex;
        public byte State;
        public int ClientID;

        public static int Size() { return 6; }
    }

    public class InitPacket
    {
        public byte PortType;
        public byte ClientIndex;
        public byte UseGamePort;
        public uint LocalIp;
        public ushort LocalPort;

        public static int Size() { return 9; }
    }

    public class ReportPacket
    {
        public byte PortType;
        public byte ClientIndex;
        public byte NegResult;
        public NatType NatType; //int
        public NatMappingScheme NatMappingScheme; //int
        public byte[] GameName = new byte[50];

        public static int Size() { return 61; }
    }

    public class ConnectPacket
    {
        public uint RemoteIP;
        public ushort RemotePort;
        public byte GotYourData;
        public byte Finished;

        public static int Size() { return 8; }
    }
}
