using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class GETCKEYDataModel
    {
        public string NickName { get; set; }
        public string UserValues { get; set; }
    }
    internal sealed class GetCKeyResult : ResultBase
    {
        public List<GETCKEYDataModel> DataResults { get; }
        public string ChannelName { get; set; }
        public GetCKeyResult()
        {
            DataResults = new List<GETCKEYDataModel>();
        }
    }
}
