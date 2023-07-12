using System;
using System.Collections.Generic;
using UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;
using UniSpy.Server.NatNegotiation.Aggregate.Redis.Fail;

namespace UniSpy.Server.NatNegotiation.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<RelayServerInfo> GetAvaliableRelayServers();
        void UpdateInitInfo(NatAddressInfo info);
        int CountInitInfo(uint cookie, byte version);
        List<NatAddressInfo> GetInitInfos(Guid serverId, uint cookie);
        void RemoveInitInfo(NatAddressInfo info);
        void UpdateNatFailInfo(NatFailInfo info);
        int GetNatFailInfo(NatFailInfo info);
    }
}