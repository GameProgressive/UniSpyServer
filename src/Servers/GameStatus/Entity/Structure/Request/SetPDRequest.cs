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
    public class SetPDRequest : GSRequestBase
    {
        public SetPDRequest(string request) : base(request)
        {
        }
        public uint ProfileID { get; protected set; }
        public PersistStorageType StorageType { get; protected set; }
        public uint DataIndex { get; protected set; }
        public uint Length { get; protected set; }
        public string KeyValueString { get; protected set; }

        public override object Parse()
        {
           var flag = (GSError)base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }

            if (!RequestKeyValues.ContainsKey("pid") || !RequestKeyValues.ContainsKey("ptype")
                || !RequestKeyValues.ContainsKey("dindex") || !RequestKeyValues.ContainsKey("length"))
            {
                return GSError.Parse;
            }

            uint profileID;
            if (!uint.TryParse(RequestKeyValues["pid"], out profileID))
            {
                return GSError.Parse;
            }
            ProfileID = profileID;

            uint storageType;
            if (!uint.TryParse(RequestKeyValues["ptype"], out storageType))
            {
                return GSError.Parse;
            }

            if (!Enum.IsDefined(typeof(PersistStorageType), storageType))
            {
                return GSError.Parse;
            }

            StorageType = (PersistStorageType)storageType;

            uint dindex;
            if (!uint.TryParse(RequestKeyValues["dindex"], out dindex))
            {
                return GSError.Parse;
            }
            DataIndex = dindex;

            uint length;
            if (!uint.TryParse(RequestKeyValues["length"], out length))
            {
                return GSError.Parse;
            }
            Length = length;

            //we extract the key value data
            foreach (var d in RequestKeyValues.Skip(5))
            {
                if (d.Key == "lid")
                    break;
                KeyValueString += @"\" + d.Key + @"\" + d.Value;
            }

            return GSError.NoError;
        }
    }
}
