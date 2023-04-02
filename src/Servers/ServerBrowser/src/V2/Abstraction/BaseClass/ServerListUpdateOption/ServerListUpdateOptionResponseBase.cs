using System.Collections.Generic;
using UniSpy.Server.ServerBrowser.V2.Application;
using UniSpy.Server.ServerBrowser.V2.Enumerate;
using UniSpy.Server.ServerBrowser.V2.Aggregate.Misc;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass
{
    public abstract class ServerListUpdateOptionResponseBase : ResponseBase
    {
        protected new ServerListUpdateOptionRequestBase _request => (ServerListUpdateOptionRequestBase)base._request;
        protected new ServerListUpdateOptionResultBase _result => (ServerListUpdateOptionResultBase)base._result;
        protected List<byte> _serversInfoBuffer;
        public ServerListUpdateOptionResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
            _serversInfoBuffer = new List<byte>();
        }

        public override void Build()
        {
            //todo check protocol version to build response data
            //Add crypt header
            BuildCryptHeader();
            _serversInfoBuffer.AddRange(_result.ClientRemoteIP);
            _serversInfoBuffer.AddRange(ClientInfo.HtonQueryReportDefaultPort);
        }
        protected void BuildCryptHeader()
        {
            // cryptHeader have 14 bytes, when we encrypt data we need skip the first 14 bytes
            var cryptHeader = new List<byte>();
            cryptHeader.Add(2 ^ 0xEC);
            #region message length?
            cryptHeader.AddRange(new byte[] { 0, 0 });
            #endregion
            cryptHeader.Add((byte)(ClientInfo.ServerChallenge.Length ^ 0xEA));
            cryptHeader.AddRange(UniSpyEncoding.GetBytes(ClientInfo.ServerChallenge));
            _serversInfoBuffer.AddRange(cryptHeader);
        }
        protected abstract void BuildServersFullInfo();

        protected void BuildUniqueValue()
        {
            //because we are using NTS string so we do not have any value here
            _serversInfoBuffer.Add(0);
        }
        protected void BuildServerKeys()
        {
            //we add the total number of the requested keys
            _serversInfoBuffer.Add((byte)_request.Keys.Length);
            //then we add the keys
            foreach (var key in _request.Keys)
            {
                _serversInfoBuffer.Add((byte)DataKeyType.String);
                _serversInfoBuffer.AddRange(UniSpyEncoding.GetBytes(key));
                _serversInfoBuffer.Add(StringFlag.StringSpliter);
            }
        }
    }
}
