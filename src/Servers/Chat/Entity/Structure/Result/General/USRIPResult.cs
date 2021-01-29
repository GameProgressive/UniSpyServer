using Chat.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class USRIPResult : ChatResultBase
    {
        public string RemoteIPAddress { get; set; }
        public USRIPResult()
        {
        }
    }
}
