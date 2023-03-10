using UniSpy.Server.ServerBrowser.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.Aggregate.Misc;
using UniSpy.Server.ServerBrowser.Contract.Request;
using UniSpy.Server.ServerBrowser.Contract.Result;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.ServerBrowser.Aggregate.Packet.Response
{
    public sealed class ServerMainListResponse : ServerListUpdateOptionResponseBase
    {
        private new ServerListRequest _request => (ServerListRequest)base._request;
        private new ServerMainListResult _result => (ServerMainListResult)base._result;
        public ServerMainListResponse(ServerListUpdateOptionRequestBase request, ServerListUpdateOptionResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            // we add the other header
            base.Build();
            // we add the server keys
            BuildServerKeys();
            //we use NTS string so total unique value list is 0
            BuildUniqueValue();
            //add server infomation such as public ip etc.
            BuildServersFullInfo();

            SendingBuffer = _serversInfoBuffer.ToArray();
        }

        protected override void BuildServersFullInfo()
        {
            foreach (var serverInfo in _result.GameServerInfos)
            {
                var header = ServerInfoBuilder.BuildServerInfoHeader(_result.Flag, serverInfo);
                _serversInfoBuffer.AddRange(header);
                foreach (var key in _request.Keys)
                {
                    _serversInfoBuffer.Add(StringFlag.NTSStringFlag);
                    // if the key is in our database, we just add it
                    // otherwise we leave it empty, game will set default value
                    if (serverInfo.ServerData.ContainsKey(key))
                    {
                        _serversInfoBuffer.AddRange(UniSpyEncoding.GetBytes(serverInfo.ServerData[key]));
                    }
                    _serversInfoBuffer.Add(StringFlag.StringSpliter);
                }
            }
            //after all server information is added we add the end flag
            _serversInfoBuffer.AddRange(StringFlag.AllServerEndFlag);
        }
    }
}
