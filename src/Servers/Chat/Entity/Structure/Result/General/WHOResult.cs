using Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class WHODataModel
    {
        public string ChannelName { get; set; }
        public string UserName { get; set; }
        public string PublicIPAddress { get; set; }
        public string NickName { get; set; }
        public string Modes { get; set; }
    }
    internal sealed class WHOResult : ChatResultBase
    {
        public List<WHODataModel> DataModels { get; set; }

        public WHOResult()
        {
            DataModels = new List<WHODataModel>();
        }
    }
}
