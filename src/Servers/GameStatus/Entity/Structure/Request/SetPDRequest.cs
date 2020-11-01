using StatsTracking.Abstraction.BaseClass;
using StatsTracking.Entity.Enumerate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatsTracking.Entity.Structure.Request
{
    public class SetPDRequest : STRequestBase
    {
        public SetPDRequest(Dictionary<string, string> request) : base(request)
        {
        }
        public uint ProfileID { get; protected set; }
        public PersistStorageType StorageType { get; protected set; }
        public uint DataIndex { get; protected set; }
        public uint Length { get; protected set; }
        public string KeyValueString { get; protected set; }

        public override STError Parse()
        {
            var flag = base.Parse();
            if (flag != STError.NoError)
            {
                return flag;
            }

            if (!_request.ContainsKey("pid") || !_request.ContainsKey("ptype") || !_request.ContainsKey("dindex") || !_request.ContainsKey("length"))
            {
                return STError.Parse;
            }

            uint profileID;
            if (!uint.TryParse(_request["pid"], out profileID))
            {
                return STError.Parse;
            }
            ProfileID = profileID;

            uint storageType;
            if (!uint.TryParse(_request["ptype"], out storageType))
            {
                return STError.Parse;
            }

            if (!Enum.IsDefined(typeof(PersistStorageType), storageType))
            {
                return STError.Parse;
            }

            StorageType = (PersistStorageType)storageType;

            uint dindex;
            if (!uint.TryParse(_request["dindex"], out dindex))
            {
                return STError.Parse;
            }
            DataIndex = dindex;

            uint length;
            if (!uint.TryParse(_request["length"], out length))
            {
                return STError.Parse;
            }
            Length = length;

            //we extract the key value data
            foreach (var d in _request.Skip(5))
            {
                if (d.Key == "lid")
                    break;
                KeyValueString += @"\" + d.Key + @"\" + d.Value;
            }

            return STError.NoError;
        }
    }
}
