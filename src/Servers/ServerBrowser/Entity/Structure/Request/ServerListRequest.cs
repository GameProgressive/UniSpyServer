using ServerBrowser.Entity.Enumerate;
using System;
using System.Linq;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Request
{
    /// <summary>
    /// ServerList also called ServerRule
    /// </summary>
    public class ServerListRequest : UniSpyRequestBase
    {
        public const ushort QueryReportDefaultPort = 6500;
        public byte RequestVersion { get; protected set; }
        public byte ProtocolVersion { get; protected set; }
        public byte EncodingVersion { get; protected set; }
        public int GameVersion { get; protected set; }
        public int QueryOptions { get; protected set; }

        public string DevGameName { get; protected set; }
        public string GameName { get; protected set; }
        public string Challenge { get; protected set; }
        public SBServerListUpdateOption UpdateOption { get; protected set; }

        public string[] Keys { get; protected set; }
        public string Filter;
        public byte[] SourceIP { get; protected set; }
        public int MaxServers { get; protected set; }

        public new byte[] RawRequest
        {
            get { return (byte[])base.RawRequest; }
            protected set { base.RawRequest = value; }
        }

        public new SBClientRequestType CommandName
        {
            get { return (SBClientRequestType)base.CommandName; }
            protected set { base.CommandName = value; }
        }

        public ServerListRequest(byte[] rawRequest) : base(rawRequest)
        {
            SourceIP = new byte[4];
            CommandName = SBClientRequestType.ServerListRequest;
        }

        /// <summary>
        /// Parse all value to this class
        /// </summary>
        /// <param name="recv"></param>
        public override void Parse()
        {
            ushort length = ByteTools.ToUInt16(ByteTools.SubBytes(RawRequest, 0, 2), true);

            if (length != RawRequest.Length)
            {
                ErrorCode = false;
                return;
            }

            RequestVersion = RawRequest[2];
            ProtocolVersion = RawRequest[3];
            EncodingVersion = RawRequest[4];
            GameVersion = BitConverter.ToInt32(ByteTools.SubBytes(RawRequest, 5, 4));

            //because there are empty string we can not use StringSplitOptions.RemoveEmptyEntries
            string remainData = Encoding.ASCII.GetString(RawRequest.Skip(9).ToArray());
            remainData.IndexOf('\0');
            DevGameName = remainData.Substring(0, remainData.IndexOf('\0'));
            remainData = remainData.Substring(remainData.IndexOf('\0') + 1);
            GameName = remainData.Substring(0, remainData.IndexOf('\0'));
            remainData = remainData.Substring(remainData.IndexOf('\0') + 1);
            Challenge = remainData.Substring(0, remainData.IndexOf('\0')).Substring(0, 8);

            if (remainData.Substring(0, remainData.IndexOf('\0')).Length > 8)
            {
                Filter = remainData.Substring(8, remainData.IndexOf('\0') - 8);
            }

            remainData = remainData.Substring(remainData.IndexOf('\0') + 1);
            Keys = remainData.Substring(0, remainData.IndexOf('\0')).Split("\\", StringSplitOptions.RemoveEmptyEntries);
            remainData = remainData.Substring(remainData.IndexOf('\0') + 1);

            byte[] byteUpdateOptions = Encoding.ASCII.GetBytes(remainData.Substring(0, 4));
            //gamespy send this in big endian, we need to convert to little endian
            Array.Reverse(byteUpdateOptions);

            UpdateOption = (SBServerListUpdateOption)BitConverter.ToInt32(byteUpdateOptions);

            if ((UpdateOption & SBServerListUpdateOption.AlternateSourceIP) != 0)
            {
                SourceIP = Encoding.ASCII.GetBytes(remainData.Substring(0, 4));
                remainData = remainData.Substring(7);
            }

            if ((UpdateOption & SBServerListUpdateOption.LimitResultCount) != 0)
            {
                MaxServers = ByteTools.ToInt32(remainData.Substring(0, 4), true);
            }

            ErrorCode = true;
        }
    }
}
