using System;
using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result
{
    internal sealed class MODEResult : ChatResultBase
    {
        public string ChannelModes { get; set; }
        public string ChannelName { get; set; }
        public string NickName { get; set; }
        public MODEResult()
        {
        }
    }
}
