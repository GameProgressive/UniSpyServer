using System;
using System.Net;
using System.Text;

namespace GameSpyLib.Extensions
{
    public static class ByteTools
    {
        /// <summary>
        /// Converts the byte array to its Hex string equivlent
        /// </summary>
        /// <param name="upperCase">Do we uppercase the hex string?</param>
        /// <returns></returns>
        public static string ToHex(this byte[] bytes, bool upperCase = true)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }

        public static byte[] SubBytes(byte[] srcBytes, int startIndex, int length)
        {
            byte[] temp = new byte[length];
            Array.Copy(srcBytes, startIndex, temp, 0, length);
            return temp;
        }

        public static byte[] GetPortBytes(string port)
        {
            byte[] bytePort = BitConverter.GetBytes(ushort.Parse(port));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytePort);
            }
            return bytePort;
        }

        public static byte[] GetPortBytes(IPEndPoint endPoint)
        {
            byte[] portBytes = BitConverter.GetBytes((short)endPoint.Port);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(portBytes);
            }
            return portBytes;
        }
        public static byte[] GetPortBytes(EndPoint endPoint)
        {
            return GetPortBytes((IPEndPoint)endPoint);
        }

        public static byte[] GetIPBytes(string ip)
        {
            return IPAddress.Parse(ip).GetAddressBytes();
        }

        public static byte[] GetIPBytes(IPEndPoint endPoint)
        {
            return endPoint.Address.GetAddressBytes();
        }

        public static byte[] GetIPBytes(EndPoint endPoint)
        {
            return GetIPBytes((IPEndPoint)endPoint);
        }

        public static IPEndPoint GetIPEndFromIPAndPort(byte[] ip, byte[] port)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(port);
            }
            return new IPEndPoint(BitConverter.ToInt32(ip), BitConverter.ToInt32(port));
        }

        public static short GetMessageLength(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }
            return BitConverter.ToInt16(data);
        }

        public static byte[] ASCIIStrToBytes(string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }

        public static string ByteToASCIIStr(byte[] data)
        {
            return Encoding.ASCII.GetString(data);
        }

        public static int BytesToInt(byte[] data)
        {
            return BitConverter.ToInt32(data);
        }

        public static byte[] IntToBytes(int data)
        {
            return BitConverter.GetBytes(data);
        }

    }
}
