using QueryReport.Entity.Structure.Group;
using ServerBrowser.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class SendGroupResult : UpdateOptionResultBase
    {
        public PeerGroupInfo PeerGroupInfo { get; set; }
        public SendGroupResult()
        {
        }
    }
}
