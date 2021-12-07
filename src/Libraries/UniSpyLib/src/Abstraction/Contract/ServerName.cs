using System;

namespace UniSpyServer.UniSpyLib.Abstraction.Contract
{
    public class ServerNameAttribute : Attribute
    {
        public string Name { get; set; }
        public ServerNameAttribute(string name)
        {
            Name = name;
        }
    }
}