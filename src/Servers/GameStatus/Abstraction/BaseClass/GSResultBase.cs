using GameStatus.Entity.Enumerate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Abstraction.BaseClass
{
    internal abstract class GSResultBase : UniSpyResultBase
    {
        public new GSErrorCode ErrorCode 
        { 
            get { return (GSErrorCode)base.ErrorCode; } 
            set { base.ErrorCode = value; } 
        }
        public GSResultBase()
        {
        }
    }
}
