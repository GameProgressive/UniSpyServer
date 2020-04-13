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

        public static byte[] GetIPBytes(IPEndPoint endPoint, bool isReversingBytes = false)
        {
            return GetBytes(endPoint.Address.GetAddressBytes(),isReversingBytes);
        }

        public static byte[] GetIPBytes(EndPoint endPoint, bool isReversingBytes = false)
        {
            return GetIPBytes((IPEndPoint)endPoint,isReversingBytes);
        }

        public static byte[] GetIPBytes(string strIP, bool isReversingBytes = false)
        {
            byte[] ip= IPAddress.Parse(strIP).GetAddressBytes();
            return GetBytes(ip, isReversingBytes);
        }

        public static IPEndPoint GetIPEndPoint(byte[] ip, byte[] port)
        {
            return new IPEndPoint(BitConverter.ToInt32(ip), BitConverter.ToUInt16(port));
        }

        public static IPEndPoint GetIPEndPoint(int ip, ushort port)
        {
            return new IPEndPoint(ip, port);
        }

       
        public static ushort ToUInt16(string value, bool isReversingBytes = false)
        {
            ushort data = ushort.Parse(value);
            byte[] buffer = BitConverter.GetBytes(data);
            return ToUInt16(buffer, isReversingBytes);
        }

        public static ushort ToUInt16(byte[] value, bool isReversingBytes = false)
        {
            if (isReversingBytes)
            {
                Array.Reverse(value);
            }
            return BitConverter.ToUInt16(value);
        }

        public static int ToInt32(byte[] value,bool isReversingBytes = false)
        {
            if (isReversingBytes)
            {
                Array.Reverse(value);
            }
           return  BitConverter.ToInt32(value);
        }

        public static int ToInt32(string value, bool isReversingBytes = false)
        {
            int data = int.Parse(value);
            byte[] buffer = BitConverter.GetBytes(data);

            return ToInt32(buffer, isReversingBytes);
        }

        public static byte[] GetBytes(int value, bool isReversingBytes = false)
        {
           return GetBytes(BitConverter.GetBytes(value), isReversingBytes);
        }

        public static byte[] GetBytes(short value, bool isReversingBytes = false)
        {
            return GetBytes(BitConverter.GetBytes(value), isReversingBytes);
        }

        public static byte[] GetBytes(uint value, bool isReversingBytes = false)
        {
            return GetBytes(BitConverter.GetBytes(value), isReversingBytes);
        }

        public static byte[] GetBytes(ushort value, bool isReversingBytes = false)
        {
            return GetBytes(BitConverter.GetBytes(value), isReversingBytes);
        }
        public static byte[] GetBytes(byte[] data, bool isReversingBytes = false)
        {
            if (isReversingBytes)
            {
                Array.Reverse(data);
            }
            return data;
        }
    }
}
