using StatsAndTracking.Abstraction.BaseClass;
using StatsAndTracking.Entity.Enumerate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Entity.Structure.Request
{
    public class SetPDRequest : GStatsRequestBase
    {
        public SetPDRequest(Dictionary<string, string> request) : base(request)
        {
        }
        public uint ProfileID { get; protected set; }
        public PersistStorageType StorageType { get; protected set; }
        public uint DataIndex { get; protected set; }
        public uint Length { get; protected set; }
        public string KeyValueString { get; protected set; }

        public override GStatsErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GStatsErrorCode.NoError)
            {
                return flag;
            }

            if (!_request.ContainsKey("pid") || !_request.ContainsKey("ptype") || !_request.ContainsKey("dindex") || !_request.ContainsKey("length"))
            {
                return GStatsErrorCode.Parse;
            }

            uint profileID;
            if (!uint.TryParse(_request["pid"], out profileID))
            {
                return GStatsErrorCode.Parse;
            }
            ProfileID = profileID;

            uint storageType;
            if (!uint.TryParse(_request["ptype"], out storageType))
            {
                return GStatsErrorCode.Parse;
            }

            if (!Enum.IsDefined(typeof(PersistStorageType), storageType))
            {
                return GStatsErrorCode.Parse;
            }

            StorageType = (PersistStorageType)storageType;

            uint dindex;
            if (!uint.TryParse(_request["dindex"], out dindex))
            {
                return GStatsErrorCode.Parse;
            }
            DataIndex = dindex;

            uint length;
            if (!uint.TryParse(_request["length"], out length))
            {
                return GStatsErrorCode.Parse;
            }
            Length = length;

            //we extract the key value data
            foreach (var d in _request.Skip(5))
            {
                if (d.Key == "lid")
                    break;
                KeyValueString += @"\" + d.Key + @"\" + d.Value;
            }

            return GStatsErrorCode.NoError;
        }
    }
}
