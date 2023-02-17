using System;
using System.Collections.Generic;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;

namespace UniSpy.Server.NatNegotiation.Abstraction.Interface
{
    public interface IStorageOperation
    {
        void UpdateInitInfo(NatAddressInfo info);
        int CountInitInfo(uint cookie, byte version);
        List<NatAddressInfo> GetInitInfos(Guid serverId, uint cookie);
        void RemoveInitInfo(NatAddressInfo info);
        void UpdateNatFailInfo(NatNegotiation.Aggregate.Redis.Fail.NatFailInfo info);
        int GetNatFailInfo(NatNegotiation.Aggregate.Redis.Fail.NatFailInfo info);
    }
}