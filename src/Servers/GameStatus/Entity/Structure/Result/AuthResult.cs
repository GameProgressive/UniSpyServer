using GameStatus.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStatus.Entity.Structure.Result
{
    internal class AuthResult : GSResultBase
    {
        public uint SessionKey { get; set; }
        public AuthResult()
        {
        }
    }
}
