using System;

namespace UniSpyLib.Abstraction.BaseClass
{
    public class CommandAttribute : Attribute
    {
        public object Name { get; }
        public CommandAttribute(object name)
        {
            Name = name;
        }
    }
}
