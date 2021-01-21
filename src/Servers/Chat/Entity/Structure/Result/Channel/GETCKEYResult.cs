using Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result
{
    internal class GETCKEYDataModel
    {
        public string NickName { get; set; }
        public string UserValues { get; set; }
    }

    internal sealed class GETCKEYResult : ChatResultBase
    {
        public List<GETCKEYDataModel> DataResults { get; set; }
        public string ChannelName { get; set; }
        public GETCKEYResult()
        {
            DataResults = new List<GETCKEYDataModel>();
        }
    }
}
