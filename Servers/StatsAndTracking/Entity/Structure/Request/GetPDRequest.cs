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
        public GetPDRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override GStatsErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GStatsErrorCode.NoError)
            {
                return flag;
            }

            if (_request.ContainsKey("pid"))
            {
                uint profileID;
                if (!uint.TryParse(_request["pid"], out profileID))
                {
                    return GStatsErrorCode.Parse;
                }
                ProfileID = profileID;
            }

            if (_request.ContainsKey("ptype"))
            {
                PersistStorageType storageType;
                if (!Enum.TryParse(_request["ptype"], out storageType))
                {
                    return GStatsErrorCode.Parse;
                }

                StorageType = storageType;
            }


            if (_request.ContainsKey("dindex"))
            {
                uint dataIndex;
                if (!uint.TryParse(_request["dindex"], out dataIndex))
                {
                    return GStatsErrorCode.Parse;
                }
                DataIndex = dataIndex;
            }

            return GStatsErrorCode.NoError;
        }
    }
}
