using System.Collections.Generic;
using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Structure.Request.General;
using UniSpyServer.Chat.Entity.Structure.Response.General;
using UniSpyServer.Chat.Entity.Structure.Result.General;
using UniSpyServer.Chat.Handler.SystemHandler.ChannelManage;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.General
{
    //todo unfinished
    [HandlerContract("LIST")]
    public sealed class ListHandler : LogedInHandlerBase
    {
        private new ListRequest _request => (ListRequest)base._request;
        private new ListResult _result
        {
            get => (ListResult)base._result;
            set => base._result = value;
        }
        //:irc.foonet.com 321 Pants Channel :Users  Name\r\n:irc.foonet.com 323 Pants :End of /LIST\r\n
        public ListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ListResult();
        }
        protected override void DataOperation()
        {
            //add list response header
            foreach (var channel in ChatChannelManager.Channels.Values)
            {
                //TODO
                //add channel information here
                ListDataModel channelInfo = new ListDataModel
                {
                    ChannelName = channel.Property.ChannelName,
                    TotalChannelUsers = channel.Property.ChannelUsers.Count,
                    ChannelTopic = channel.Property.ChannelTopic
                };
                _result.ChannelInfoList.Add(channelInfo);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ListResponse(_request, _result);
        }
    }
}
