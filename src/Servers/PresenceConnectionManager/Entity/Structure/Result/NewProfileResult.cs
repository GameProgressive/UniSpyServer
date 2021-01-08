using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    public class NewProfileResult:PCMResultBase
    {
        public uint ProfileID;

        public NewProfileResult()
        {
        }

        public NewProfileResult(UniSpyRequestBase request) : base(request)
        {
        }
    }
}
