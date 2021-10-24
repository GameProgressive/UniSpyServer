using UniSpyServer.Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.Chat.Entity.Structure.Result.General
{
    public sealed class GetKeyResult : ResultBase
    {
        public List<string> Flags { get; }
        /// <summary>
        /// The reciever's nick name
        /// </summary>
        public string NickName { get; set; }
        public GetKeyResult()
        {
            Flags = new List<string>();
        }
    }
}
