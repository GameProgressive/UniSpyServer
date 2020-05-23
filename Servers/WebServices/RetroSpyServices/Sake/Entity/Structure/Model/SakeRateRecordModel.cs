using System;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model
{
    public class SakeRateRecordModel : SakeBaseModel
    {
        [DataMember(Name = SakeConstant.Rating)]
        public uint Rating;
    }
}
