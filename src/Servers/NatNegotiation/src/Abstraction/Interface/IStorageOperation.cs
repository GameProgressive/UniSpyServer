using System;
using System.Collections.Generic;
using UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;
using UniSpy.Server.NatNegotiation.Aggregate.Redis.Fail;

namespace UniSpy.Server.NatNegotiation.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<RelayServerCache> GetAvaliableRelayServers();
        void UpdateInitInfo(InitPacketCache info);
        int CountInitInfo(uint cookie, byte version);
        List<InitPacketCache> GetInitInfos(Guid serverId, uint cookie);
        void RemoveInitInfo(InitPacketCache info);
        void UpdateNatFailInfo(NatFailInfoCache info);
        int GetNatFailInfo(NatFailInfoCache info);
    }
}