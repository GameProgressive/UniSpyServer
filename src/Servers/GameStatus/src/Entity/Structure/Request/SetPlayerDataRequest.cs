using UniSpyServer.GameStatus.Abstraction.BaseClass;
using UniSpyServer.GameStatus.Entity.Contract;
using UniSpyServer.GameStatus.Entity.Enumerate;
using UniSpyServer.GameStatus.Entity.Exception;
using System;
using System.Linq;

namespace UniSpyServer.GameStatus.Entity.Structure.Request
{
    /// <summary>
    /// "\setpd\\pid\4\ptype\4\dindex\4\kv\\key1\value1\key2\value2\key3\value3\lid\2\length\5\data\final\"
    /// </summary>
    [RequestContract("setpd")]
    
    public sealed class SetPlayerDataRequest : RequestBase
    {
        public SetPlayerDataRequest(string request) : base(request)
        {
        }
        public uint ProfileID { get; private set; }
        public PersistStorageType StorageType { get; private set; }
        public uint DataIndex { get; private set; }
        public uint Length { get; private set; }
        public string KeyValueString { get; private set; }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("pid"))
                throw new GSException("length is missing.");

            if (!RequestKeyValues.ContainsKey("ptype"))
                throw new GSException("length is missing.");

            if (!RequestKeyValues.ContainsKey("dindex"))
                throw new GSException("length is missing.");

            if (!RequestKeyValues.ContainsKey("length"))
                throw new GSException("length is missing.");


            uint profileID;
            if (!uint.TryParse(RequestKeyValues["pid"], out profileID))
            {
                throw new GSException("pid format is incorrect.");
            }
            ProfileID = profileID;

            uint storageType;
            if (!uint.TryParse(RequestKeyValues["ptype"], out storageType))
            {
                throw new GSException("ptype is missing.");
            }

            if (!Enum.IsDefined(typeof(PersistStorageType), storageType))
            {
                throw new GSException("storage type is incorrect.");
            }

            StorageType = (PersistStorageType)storageType;

            uint dindex;
            if (!uint.TryParse(RequestKeyValues["dindex"], out dindex))
            {
                throw new GSException("dindex format is incorrect.");
            }
            DataIndex = dindex;

            uint length;
            if (!uint.TryParse(RequestKeyValues["length"], out length))
            {
                throw new GSException("length format is incorrect.");
            }
            Length = length;

            //we extract the key value data
            foreach (var d in RequestKeyValues.Skip(5))
            {
                if (d.Key == "lid")
                    break;
                KeyValueString += @"\" + d.Key + @"\" + d.Value;
            }
        }
    }
}
