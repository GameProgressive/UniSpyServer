using Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result.Channel
{
    internal class GETKEYResult : ChatResultBase
    {
        public List<string> Flags { get; }
        /// <summary>
        /// The reciever's nick name
        /// </summary>
        public string NickName { get; set; }
        public GETKEYResult()
        {
            Flags = new List<string>();
        }
    }
}
