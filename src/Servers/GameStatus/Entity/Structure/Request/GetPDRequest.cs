using StatsTracking.Abstraction.BaseClass;
using StatsTracking.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace StatsTracking.Entity.Structure.Request
{

    public class GetPDRequest : STRequestBase
    {
        public uint ProfileID { get; protected set; }
        public PersistStorageType StorageType { get; protected set; }
        public uint DataIndex { get; protected set; }
        public List<string> Keys { get; protected set; }
        public bool GetAllDataFlag { get; protected set; }
        public GetPDRequest(Dictionary<string, string> request) : base(request)
        {
            Keys = new List<string>();
        }

        public override STError Parse()
        {
            var flag = base.Parse();
            if (flag != STError.NoError)
            {
                return flag;
            }

            if (_request.ContainsKey("pid"))
            {
                uint profileID;
                if (!uint.TryParse(_request["pid"], out profileID))
                {
                    return STError.Parse;
                }
                ProfileID = profileID;
            }

            if (_request.ContainsKey("ptype"))
            {
                PersistStorageType storageType;
                if (!Enum.TryParse(_request["ptype"], out storageType))
                {
                    return STError.Parse;
                }

                StorageType = storageType;
            }


            if (_request.ContainsKey("dindex"))
            {
                uint dataIndex;
                if (!uint.TryParse(_request["dindex"], out dataIndex))
                {
                    return STError.Parse;
                }
                DataIndex = dataIndex;
            }

            if (!_request.ContainsKey("keys"))
            {
                return STError.Parse;
            }

            string keys = _request["keys"];
            if (keys == "")
            {
                GetAllDataFlag = true;
            }
            else
            {
                string[] keyArray = keys.Split('\x1');
                foreach (var key in keyArray)
                {
                    Keys.Add(key);
                }
                GetAllDataFlag = false;
            }

            return STError.NoError;
        }
    }
}
