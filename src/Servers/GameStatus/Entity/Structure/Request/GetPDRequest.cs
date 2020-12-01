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
        public GetPDRequest(Dictionary<string, string> request) : base(request)
        {
            Keys = new List<string>();
        }

        public override GSError Parse()
        {
            var flag = base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }

            if (_rawRequest.ContainsKey("pid"))
            {
                uint profileID;
                if (!uint.TryParse(_rawRequest["pid"], out profileID))
                {
                    return GSError.Parse;
                }
                ProfileID = profileID;
            }

            if (_rawRequest.ContainsKey("ptype"))
            {
                PersistStorageType storageType;
                if (!Enum.TryParse(_rawRequest["ptype"], out storageType))
                {
                    return GSError.Parse;
                }

                StorageType = storageType;
            }


            if (_rawRequest.ContainsKey("dindex"))
            {
                uint dataIndex;
                if (!uint.TryParse(_rawRequest["dindex"], out dataIndex))
                {
                    return GSError.Parse;
                }
                DataIndex = dataIndex;
            }

            if (!_rawRequest.ContainsKey("keys"))
            {
                return GSError.Parse;
            }

            string keys = _rawRequest["keys"];
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
