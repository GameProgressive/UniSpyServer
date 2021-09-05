using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Exception;
using System;
using System.Linq;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Request
{
    /// <summary>
    /// ServerList also called ServerRule in GameSpy SDK
    /// </summary>
    internal sealed class ServerListRequest : ServerListUpdateOptionRequestBase
    {
        public ServerListRequest(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            CommandName = SBClientRequestType.ServerListRequest;
            if (RequestLength != RawRequest.Length)
            {
                throw new SBException("Server list request length is not correct.");
            }

            RequestVersion = RawRequest[2];
            ProtocolVersion = RawRequest[3];
            EncodingVersion = RawRequest[4];
            GameVersion = BitConverter.ToInt32(ByteTools.SubBytes(RawRequest, 5, 4));

            //because there are empty string we can not use StringSplitOptions.RemoveEmptyEntries
            string remainData = UniSpyEncoding.GetString(RawRequest.Skip(9).ToArray());
            remainData.IndexOf('\0');
            DevGameName = remainData.Substring(0, remainData.IndexOf('\0'));
            remainData = remainData.Substring(remainData.IndexOf('\0') + 1);
            GameName = remainData.Substring(0, remainData.IndexOf('\0'));
            remainData = remainData.Substring(remainData.IndexOf('\0') + 1);
            ClientChallenge = remainData.Substring(0, remainData.IndexOf('\0')).Substring(0, 8);

            if (remainData.Substring(0, remainData.IndexOf('\0')).Length > 8)
            {
                Filter = remainData.Substring(8, remainData.IndexOf('\0') - 8);
            }

            remainData = remainData.Substring(remainData.IndexOf('\0') + 1);
            Keys = remainData.Substring(0, remainData.IndexOf('\0')).Split("\\", StringSplitOptions.RemoveEmptyEntries);
            remainData = remainData.Substring(remainData.IndexOf('\0') + 1);

            byte[] byteUpdateOptions = UniSpyEncoding.GetBytes(remainData.Substring(0, 4));
            //gamespy send this in big endian, we need to convert to little endian
            Array.Reverse(byteUpdateOptions);

            UpdateOption = (SBServerListUpdateOption)BitConverter.ToInt32(byteUpdateOptions);

            if ((UpdateOption & SBServerListUpdateOption.AlternateSourceIP) != 0)
            {
                SourceIP = UniSpyEncoding.GetBytes(remainData.Substring(0, 4));
                remainData = remainData.Substring(7);
            }

            if ((UpdateOption & SBServerListUpdateOption.LimitResultCount) != 0)
            {
                MaxServers = ByteTools.ToInt32(remainData.Substring(0, 4), true);
            }
        }
    }
}
