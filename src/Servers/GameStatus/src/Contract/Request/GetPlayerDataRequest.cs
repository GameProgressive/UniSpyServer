using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Enumerate;

using System;
using System.Collections.Generic;

namespace UniSpy.Server.GameStatus.Contract.Request
{
    
    public sealed class GetPlayerDataRequest : RequestBase
    {
        public int ProfileId { get; private set; }
        public PersistStorageType StorageType { get; private set; }
        public int DataIndex { get; private set; }
        public List<string> Keys { get; private set; }
        public bool GetAllDataFlag { get; private set; }
        public GetPlayerDataRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public override void Parse()
        {
            base.Parse();
            if (!KeyValues.ContainsKey("lid") && !KeyValues.ContainsKey("id"))
            {
                throw new GameStatus.Exception("localid is missing.");
            }

            if (KeyValues.ContainsKey("pid"))
            {
                int profileID;
                if (!int.TryParse(KeyValues["pid"], out profileID))
                {
                    throw new GameStatus.Exception("pid format is incorrect.");
                }
                ProfileId = profileID;
            }

            if (KeyValues.ContainsKey("ptype"))
            {
                PersistStorageType storageType;
                if (!Enum.TryParse(KeyValues["ptype"], out storageType))
                {
                    throw new GameStatus.Exception("ptype format is incorrect.");
                }
                StorageType = storageType;
            }


            if (KeyValues.ContainsKey("dindex"))
            {
                int dataIndex;
                if (!int.TryParse(KeyValues["dindex"], out dataIndex))
                {
                    throw new GameStatus.Exception("dindex format is incorrect.");
                }
                DataIndex = dataIndex;
            }

            if (!KeyValues.ContainsKey("keys"))
            {
                throw new GameStatus.Exception("keys is missing.");
            }

            string keys = KeyValues["keys"];
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
        }
    }
}
