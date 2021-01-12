using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Entity.Structure.Request
{
    /// <summary>
    /// "\setpd\\pid\4\ptype\4\dindex\4\kv\\key1\value1\key2\value2\key3\value3\lid\2\length\5\data\final\"
    /// </summary>
    internal sealed class SetPDRequest : GSRequestBase
    {
        public SetPDRequest(string request) : base(request)
        {
        }
        public uint ProfileID { get; protected set; }
        public PersistStorageType StorageType { get; protected set; }
        public uint DataIndex { get; protected set; }
        public uint Length { get; protected set; }
        public string KeyValueString { get; protected set; }

        public override void Parse()
        {
           base.Parse();
            if (ErrorCode != GSErrorCode.NoError)
            {
                return;
            }

            if (!RequestKeyValues.ContainsKey("pid") || !RequestKeyValues.ContainsKey("ptype")
                || !RequestKeyValues.ContainsKey("dindex") || !RequestKeyValues.ContainsKey("length"))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }

            uint profileID;
            if (!uint.TryParse(RequestKeyValues["pid"], out profileID))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }
            ProfileID = profileID;

            uint storageType;
            if (!uint.TryParse(RequestKeyValues["ptype"], out storageType))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }

            if (!Enum.IsDefined(typeof(PersistStorageType), storageType))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }

            StorageType = (PersistStorageType)storageType;

            uint dindex;
            if (!uint.TryParse(RequestKeyValues["dindex"], out dindex))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }
            DataIndex = dindex;

            uint length;
            if (!uint.TryParse(RequestKeyValues["length"], out length))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }
            Length = length;

            //we extract the key value data
            foreach (var d in RequestKeyValues.Skip(5))
            {
                if (d.Key == "lid")
                    break;
                KeyValueString += @"\" + d.Key + @"\" + d.Value;
            }

            ErrorCode = GSErrorCode.NoError;
        }
    }
}
