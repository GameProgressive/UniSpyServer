using Chat.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class LOGINResult : ChatResultBase
    {
        public uint ProfileID { get; set; }
        public uint UserID { get; set; }
        public LOGINResult()
        {
        }
    }
}
