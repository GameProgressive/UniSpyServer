using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Extensions;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class ServerListRequestBase : SBRequestBase
    {
        public static byte[] HtonQueryReportDefaultPort => ByteTools.GetBytes(QueryReportDefaultPort, true);
        public static ushort QueryReportDefaultPort = 6500;
        public byte RequestVersion { get; protected set; }
        public byte ProtocolVersion { get; protected set; }
        public byte EncodingVersion { get; protected set; }
        public int GameVersion { get; protected set; }
        public int QueryOptions { get; protected set; }

        public string DevGameName { get; protected set; }
        public string GameName { get; protected set; }
        public string ClientChallenge { get; protected set; }
        public SBServerListUpdateOption UpdateOption { get; protected set; }

        public string[] Keys { get; protected set; }
        public string Filter { get; protected set; }
        public byte[] SourceIP { get; protected set; }
        public int MaxServers { get; protected set; }

        protected ServerListRequestBase(object rawRequest) : base(rawRequest)
        {
            SourceIP = new byte[4];
        }


    }
}
