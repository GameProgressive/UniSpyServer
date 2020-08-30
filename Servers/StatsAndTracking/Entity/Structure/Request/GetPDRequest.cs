using System;
using System.Collections.Generic;
using StatsAndTracking.Entity.Enumerator;

namespace StatsAndTracking.Entity.Structure.Request
{
    public class GetPDRequest : GStatsRequestBase
    {
        public uint ProfileID { get; protected set; }
        public PersistStorageType StorageType { get; protected set; }
        public uint DataIndex { get; protected set; }
        public GetPDRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GStatsErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GStatsErrorCode.NoError)
            {
                return flag;
            }

            if (_recv.ContainsKey("pid"))
            {
                uint profileID;
                if (!uint.TryParse(_recv["pid"], out profileID))
                {
                    return GStatsErrorCode.Parse;
                }
                ProfileID = profileID;
            }

            if (_recv.ContainsKey("ptype"))
            {
                uint storageType;
                if (!Enum.TryParse(_recv["ptype"], out storageType))
                {
                    return GStatsErrorCode.Parse;
                }

                if (!Enum.IsDefined(typeof(PersistStorageType), storageType))
                {
                    return GStatsErrorCode.Parse;
                }

                StorageType = (PersistStorageType)storageType;
            }


            if (_recv.ContainsKey("dindex"))
            {
                uint dataIndex;
                if (!uint.TryParse(_recv["dindex"], out dataIndex))
                {
                    return GStatsErrorCode.Parse;
                }
                DataIndex = dataIndex;
            }

            return GStatsErrorCode.NoError;
        }
    }
}
