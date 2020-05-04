using System;
using System.Net;

namespace GameSpyLib.Extensions
{
    public class HtonsExtensions
    {
        public static byte[] PortToHtonBytes(ushort port)
        {
            byte[] buffer = BitConverter.GetBytes(port);
            Array.Reverse(buffer);
            return buffer;
        }

        public static byte[] PortToHtonBytes(string portStr)
        {
            ushort port = ushort.Parse(portStr);
            return PortToHtonBytes(port);
        }

        public static byte[] PortToBytes(string portStr)
        {
            ushort port = ushort.Parse(portStr);
            return PortToBytes(port);
        }
        public static byte[] PortToBytes(ushort port)
        {
            return BitConverter.GetBytes(port);
        }

        public static byte[] IPToBytes(string ip)
        {
            return IPAddress.Parse(ip).GetAddressBytes();
        }

        public static byte[] IPToBytes(uint ip)
        {
            return BitConverter.GetBytes(ip);
        }

        public static ushort HtonBytesToPort(byte[] buffer)
        {
            Array.Reverse(buffer);
            return BytesToPort(buffer);
        }
        public static ushort BytesToPort(byte[] buffer)
        {
            return BitConverter.ToUInt16(buffer);
        }
    }
}
