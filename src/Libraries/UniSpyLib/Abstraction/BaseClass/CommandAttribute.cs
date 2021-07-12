using System;

namespace UniSpyLib.Abstraction.BaseClass
{
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// The command name that used to distinguish the post actions
        /// </summary>
        /// <value></value>
        public object Name { get; }
        public CommandAttribute(object name)
        {
            Name = name;
        }
    }
}
