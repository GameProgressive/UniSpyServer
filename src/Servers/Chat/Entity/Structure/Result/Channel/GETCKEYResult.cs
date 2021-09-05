using Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class GETCKEYDataModel
    {
        public string NickName { get; set; }
        public string UserValues { get; set; }
    }

    internal sealed class GETCKEYResult : ChatResultBase
    {
        public List<GETCKEYDataModel> DataResults { get; }
        public string ChannelName { get; set; }
        public GETCKEYResult()
        {
            DataResults = new List<GETCKEYDataModel>();
        }
    }
}
