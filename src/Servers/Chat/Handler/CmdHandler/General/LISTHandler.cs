using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.General;
using Chat.Handler.SystemHandler.ChannelManage;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    //todo unfinished
    internal sealed class LISTHandler : ChatLogedInHandlerBase
    {
        private new LISTRequest _request => (LISTRequest)base._request;
        private new LISTResult _result
        {
            get => (LISTResult)base._result;
            set => base._result = value;
        }
        //:irc.foonet.com 321 Pants Channel :Users  Name\r\n:irc.foonet.com 323 Pants :End of /LIST\r\n
        public LISTHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new LISTResult();
        }
        protected override void DataOperation()
        {
            //add list response header
            foreach (var channel in ChatChannelManager.Channels.Values)
            {
                //TODO
                //add channel information here
                LISTDataModel channelInfo = new LISTDataModel
                {
                    ChannelName = channel.Property.ChannelName,
                    TotalChannelUsers = channel.Property.ChannelUsers.Count,
                    ChannelTopic = channel.Property.ChannelTopic
                };
                _result.ChannelInfos.Add(channelInfo);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new LISTResponse(_request, _result);
        }
    }
}
