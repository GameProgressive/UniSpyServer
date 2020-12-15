using System;
using System.Collections.Generic;
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace NATNegotiation.Entity.Structure
{
    public class NatUserInfo
    {
        public string RemoteEndPoint;
        public byte Version;
        public uint Cookie;
        public byte ClientIndex;
        public NatPortType PortType;
        public List<NatPortType> PortTypes;
        public bool IsGotErtAckPacket;
        public bool IsGotAddressCheckPacket;
        public bool IsGotInitPacket;
        public bool IsSendConnectPacket;
        public bool IsGotConnectAckPacket;
        public bool IsGotReportPacket;
        public GameInfo Game;
        public string GameName;
        public DateTime InitPacketRecieveTime;
        public DateTime ConnectPacketSendTime;
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;

        public NatUserInfo()
        {
            PortTypes = new List<NatPortType>();
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
            UpdatePortType(init.PortType);
            ClientIndex = init.ClientIndex;
        }

        public void UpdatePortType(NatPortType portType)
        {
            if (!PortTypes.Contains(portType))
            {
                PortTypes.Add(portType);
            }
            else
            {
                LogWriter.ToLog($"Ignore same port type{portType}");
            }
            LastPacketRecieveTime = DateTime.Now;
        }


        public static NatUserInfo GetNatUserInfo(EndPoint endPoint, uint cookie)
        {
            string key = GenerateKey(endPoint, cookie);
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
            List<string> matchedKeys = RedisExtensions.GetMatchedKeys(subKey, RedisDBNumber.GameServer);
            List<NatUserInfo> natUserInfos = new List<NatUserInfo>();
            foreach (var key in matchedKeys)
            {
                natUserInfos.Add(RedisExtensions.GetValue<NatUserInfo>(key, RedisDBNumber.GameServer));
            }
            return natUserInfos;
        }

        public static bool SetNatUserInfo(EndPoint endPoint, uint cookie, NatUserInfo userInfo)
        {
            string key = GenerateKey(endPoint, cookie);
            return SetNatUserInfo(key, userInfo);
        }

        public static bool SetNatUserInfo(string key, NatUserInfo userInfo)
        {
            return RedisExtensions.SetKeyValue(key, userInfo, RedisDBNumber.NatNeg);
        }

        public static string GenerateKey(EndPoint endPoint, uint cookie)
        {
            return $"{((IPEndPoint)endPoint)} {cookie}";
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

        public static bool DeleteNatUserInfo(EndPoint endPoint, uint cookie)
        {
            string key = GenerateKey(endPoint, cookie);
            return DeleteNatUserInfo(key);
        }

        public static bool DeleteNatUserInfo(string key)
        {
            return RedisExtensions.DeleteKeyValue(key, RedisDBNumber.NatNeg);
        }
    }
}
