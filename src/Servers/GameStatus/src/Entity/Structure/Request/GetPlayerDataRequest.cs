using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Entity.Enumerate;
using UniSpy.Server.GameStatus.Entity.Exception;
using System;
using System.Collections.Generic;

namespace UniSpy.Server.GameStatus.Entity.Structure.Request
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


            if (PlayerData.ContainsKey("pid"))
            {
                int profileID;
                if (!int.TryParse(PlayerData["pid"], out profileID))
                {
                    throw new GSException("pid format is incorrect.");
                }
                ProfileId = profileID;
            }

            if (PlayerData.ContainsKey("ptype"))
            {
                PersistStorageType storageType;
                if (!Enum.TryParse(PlayerData["ptype"], out storageType))
                {
                    throw new GSException("ptype format is incorrect.");
                }
                StorageType = storageType;
            }


            if (PlayerData.ContainsKey("dindex"))
            {
                int dataIndex;
                if (!int.TryParse(PlayerData["dindex"], out dataIndex))
                {
                    throw new GSException("dindex format is incorrect.");
                }
                DataIndex = dataIndex;
            }

            if (!PlayerData.ContainsKey("keys"))
            {
                throw new GSException("keys is missing.");
            }

            string keys = PlayerData["keys"];
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
