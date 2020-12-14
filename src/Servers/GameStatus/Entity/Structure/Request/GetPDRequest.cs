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

        public override void Parse()
        {
           base.Parse();
            if (ErrorCode != GSErrorCode.NoError)
            {
                return;
            }

            if (RequestKeyValues.ContainsKey("pid"))
            {
                uint profileID;
                if (!uint.TryParse(RequestKeyValues["pid"], out profileID))
                {
                    ErrorCode = GSErrorCode.Parse;
                    return;
                }
                ProfileID = profileID;
            }

            if (RequestKeyValues.ContainsKey("ptype"))
            {
                PersistStorageType storageType;
                if (!Enum.TryParse(RequestKeyValues["ptype"], out storageType))
                {
                    ErrorCode = GSErrorCode.Parse;
                    return;
                }

                StorageType = storageType;
            }


            if (RequestKeyValues.ContainsKey("dindex"))
            {
                uint dataIndex;
                if (!uint.TryParse(RequestKeyValues["dindex"], out dataIndex))
                {
                    ErrorCode = GSErrorCode.Parse;
                    return;
                }
                DataIndex = dataIndex;
            }

            if (!RequestKeyValues.ContainsKey("keys"))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
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

            ErrorCode = GSErrorCode.NoError;
        }
    }
}
