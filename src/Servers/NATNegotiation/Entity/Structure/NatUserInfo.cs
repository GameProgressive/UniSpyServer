using System;
using System.Collections.Generic;
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using UniSpyLib.Extensions;

namespace NATNegotiation.Entity.Structure
{
    public class NatUserInfo
    {
        public string RemoteEndPoint;
        public byte Version;
        public uint Cookie;
        public byte ClientIndex;
        public NatPortType PortType;
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;

        public NatUserInfo()
        {
        }

        public void UpdateRemoteEndPoint(EndPoint endPoint)
        {
            RemoteEndPoint = ((IPEndPoint)endPoint).ToString();
        }

        public void UpdateInitRequestInfo(InitRequest init)
        {
            Version = init.Version;
            Cookie = init.Cookie;
            PortType = init.PortType;
            ClientIndex = init.ClientIndex;
        }

        public static NatUserInfo GetNatUserInfo(string key)
        {
            return RedisExtensions.GetValue<NatUserInfo>(key, RedisDBNumber.NatNeg);
        }

        public static NatUserInfo GetNatUserInfo(EndPoint endPoint, NatPortType portType, uint cookie)
        {
            string key = GenerateKey(endPoint, portType, cookie);
            return RedisExtensions.GetValue<NatUserInfo>(key, RedisDBNumber.NatNeg);
        }

        public static NatUserInfo GetNatUserInfo(uint cookie)
        {
            return RedisExtensions.GetValue<NatUserInfo>(cookie.ToString(), RedisDBNumber.NatNeg);
        }

        public static List<NatUserInfo> GetNatUserInfos(EndPoint end)
        {
            //we build search key as 192.168.1.1:1111 format
            string subKey = ((IPEndPoint)end).ToString();
            return GetNatUserInfos(subKey);
        }

        public static List<NatUserInfo> GetNatUserInfos(string subKey)
        {
            List<string> matchedKeys = RedisExtensions.GetMatchedKeys(subKey, RedisDBNumber.NatNeg);
            List<NatUserInfo> natUserInfos = new List<NatUserInfo>();
            foreach (var key in matchedKeys)
            {
                natUserInfos.Add(RedisExtensions.GetValue<NatUserInfo>(key, RedisDBNumber.NatNeg));
            }
            return natUserInfos;
        }

        public static bool SetNatUserInfo(EndPoint endPoint,NatPortType portType, uint cookie, NatUserInfo userInfo)
        {
            return SetNatUserInfo(GenerateKey(endPoint, portType, cookie), userInfo);
        }
        public static bool SetNatUserInfo(string key, NatUserInfo userInfo)
        {
            return RedisExtensions.SetKeyValue(key, userInfo, RedisDBNumber.NatNeg);
        }

        public static List<string> GetMatchedKeys(EndPoint end, NatPortType portType, uint cookie)
        {
            string key = GenerateKey(end, portType, cookie);
            return RedisExtensions.GetMatchedKeys(key, RedisDBNumber.NatNeg);
        }

        public static List<string> GetMatchedKeys(EndPoint end, uint cookie)
        {
            string key = $"{(IPEndPoint)end}*{cookie}";
            return RedisExtensions.GetMatchedKeys(key, RedisDBNumber.NatNeg);
        }

        public static List<string> GetMatchedKeys(uint cookie)
        {
            string key = cookie.ToString();
            return RedisExtensions.GetMatchedKeys(key, RedisDBNumber.NatNeg);
        }

        public static List<string> GetMatchedKeys(EndPoint endPoint)
        {
            string address = ((IPEndPoint)endPoint).ToString();
            return RedisExtensions.GetMatchedKeys(address, RedisDBNumber.NatNeg);
        }
        public static List<string> GetMatchedKeys(string subKey)
        {
            return RedisExtensions.GetMatchedKeys(subKey, RedisDBNumber.NatNeg);
        }

        public static bool DeleteNatUserInfo(EndPoint endPoint, NatPortType portType, uint cookie)
        {
            string key = GenerateKey(endPoint, portType, cookie);
            return DeleteNatUserInfo(key);
        }

        public static bool DeleteNatUserInfo(string key)
        {
            return RedisExtensions.DeleteKeyValue(key, RedisDBNumber.NatNeg);
        }

        public static string GenerateKey(EndPoint endPoint, NatPortType portType, uint cookie)
        {
            return $"{(IPEndPoint)endPoint} {portType} {cookie}";
        }
    }
}
