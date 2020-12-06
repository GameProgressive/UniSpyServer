using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{

    public class GetPDRequest : GSRequestBase
    {
        public uint ProfileID { get; protected set; }
        public PersistStorageType StorageType { get; protected set; }
        public uint DataIndex { get; protected set; }
        public List<string> Keys { get; protected set; }
        public bool GetAllDataFlag { get; protected set; }
        public GetPDRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public override object Parse()
        {
           var flag = (GSError)base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }

            if (RequestKeyValues.ContainsKey("pid"))
            {
                uint profileID;
                if (!uint.TryParse(RequestKeyValues["pid"], out profileID))
                {
                    return GSError.Parse;
                }
                ProfileID = profileID;
            }

            if (RequestKeyValues.ContainsKey("ptype"))
            {
                PersistStorageType storageType;
                if (!Enum.TryParse(RequestKeyValues["ptype"], out storageType))
                {
                    return GSError.Parse;
                }

                StorageType = storageType;
            }


            if (RequestKeyValues.ContainsKey("dindex"))
            {
                uint dataIndex;
                if (!uint.TryParse(RequestKeyValues["dindex"], out dataIndex))
                {
                    return GSError.Parse;
                }
                DataIndex = dataIndex;
            }

            if (!RequestKeyValues.ContainsKey("keys"))
            {
                return GSError.Parse;
            }

            string keys = RequestKeyValues["keys"];
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

            return GSError.NoError;
        }
    }
}
