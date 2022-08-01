using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;

using UniSpyServer.Servers.GameStatus.Entity.Enumerate;
using UniSpyServer.Servers.GameStatus.Entity.Exception;
using System;
using System.Collections.Generic;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Request
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


            if (KeyValues.ContainsKey("pid"))
            {
                int profileID;
                if (!int.TryParse(KeyValues["pid"], out profileID))
                {
                    throw new GSException("pid format is incorrect.");
                }
                ProfileId = profileID;
            }

            if (KeyValues.ContainsKey("ptype"))
            {
                PersistStorageType storageType;
                if (!Enum.TryParse(KeyValues["ptype"], out storageType))
                {
                    throw new GSException("ptype format is incorrect.");
                }
                StorageType = storageType;
            }


            if (KeyValues.ContainsKey("dindex"))
            {
                int dataIndex;
                if (!int.TryParse(KeyValues["dindex"], out dataIndex))
                {
                    throw new GSException("dindex format is incorrect.");
                }
                DataIndex = dataIndex;
            }

            if (!KeyValues.ContainsKey("keys"))
            {
                throw new GSException("keys is missing.");
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
