using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class UpdateOptionResultBase : SBResultBase
    {
        public byte[] ClientRemoteIP { get; set; }
        protected UpdateOptionResultBase()
        {
        }
    }
}
