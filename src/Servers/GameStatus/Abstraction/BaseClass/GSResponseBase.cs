using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Abstraction.BaseClass
{
    internal abstract class GSResponseBase : UniSpyResponseBase
    {
        public GSResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
