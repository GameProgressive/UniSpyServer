﻿using UniSpyServer.ServerBrowser.Entity.Enumerate;
namespace UniSpyServer.ServerBrowser.Abstraction.BaseClass
{
    public abstract class ServerListUpdateOptionRequestBase : RequestBase
    {
        public byte RequestVersion { get; protected set; }
        public byte ProtocolVersion { get; protected set; }
        public byte EncodingVersion { get; protected set; }
        public int GameVersion { get; protected set; }
        public int QueryOptions { get; protected set; }
        public string DevGameName { get; protected set; }
        public string GameName { get; protected set; }
        public string ClientChallenge { get; protected set; }
        public ServerListUpdateOption UpdateOption { get; protected set; }
        public string[] Keys { get; protected set; }
        public string Filter { get; protected set; }
        public byte[] SourceIP { get; protected set; }
        public int MaxServers { get; protected set; }
        protected ServerListUpdateOptionRequestBase(object rawRequest) : base(rawRequest)
        {
            SourceIP = new byte[4];
        }


    }
}
