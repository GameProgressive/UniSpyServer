using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model
{
    public class SakeRecordField
    {
        [DataMember(Name = SakeConstant.Name)]
        public string Name;

        [DataMember(Name = SakeConstant.Value)]
        public SakeValue Value; 
    }
}
