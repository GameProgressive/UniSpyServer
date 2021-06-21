using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    public class CommandAttribute : Attribute
    {
        public string Name { get; }
        public CommandAttribute(string name)
        {
            Name = name;
        }
    }
}
