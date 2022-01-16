using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.Entity.Exception;
using System;
using System.Linq;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Extensions;
using System.Text;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request
{
    /// <summary>
    /// ServerList also called ServerRule in GameSpy SDK
    /// </summary>
    public sealed class ServerListRequest : ServerListUpdateOptionRequestBase
    {
        public ServerListRequest(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            CommandName = RequestType.ServerListRequest;
            if (RequestLength != RawRequest.Length)
            {
                throw new SBException("Server list request length is not correct.");
            }

            RequestVersion = RawRequest[2];
            ProtocolVersion = RawRequest[3];
            EncodingVersion = RawRequest[4];
            GameVersion = BitConverter.ToInt32(RawRequest.Skip(5).Take(4).ToArray());

            //because there are empty string we can not use StringSplitOptions.RemoveEmptyEntries
            var remainData = RawRequest.Skip(9).ToList();
            var devGameNameIndex = remainData.FindIndex(x => x == 0);
            DevGameName = UniSpyEncoding.GetString(remainData.Take(devGameNameIndex).ToArray());
            remainData = remainData.Skip(devGameNameIndex + 1).ToList();
            var gameNameIndex = remainData.FindIndex(x => x == 0);
            GameName = UniSpyEncoding.GetString(remainData.Take(gameNameIndex).ToArray());
            remainData = remainData.Skip(gameNameIndex + 1).ToList();
            var clientChallengeIndex = remainData.FindIndex(x => x == 0);
            ClientChallenge = UniSpyEncoding.GetString(remainData.Take(clientChallengeIndex).ToArray());
            var filterIndex = remainData.FindIndex(x => x == 0);
            if (remainData.Take(filterIndex).ToList().Count > 8)
            {
                Filter = UniSpyEncoding.GetString(remainData.Skip(8).Take(filterIndex).ToArray());
            }
            remainData = remainData.Skip(filterIndex + 1).ToList();

            var keysIndex = remainData.FindIndex(x => x == 0);
            Keys = UniSpyEncoding.GetString(remainData.Take(keysIndex).ToArray()).Split("\\", StringSplitOptions.RemoveEmptyEntries);
            remainData = remainData.Skip(keysIndex + 1).ToList();
            //gamespy send this in big endian, we need to convert to little endian
            byte[] byteUpdateOptions = remainData.Take(4).Reverse().ToArray();
            UpdateOption = (ServerListUpdateOption)BitConverter.ToInt32(byteUpdateOptions);
            remainData = remainData.Skip(4).ToList();
            if ((UpdateOption & ServerListUpdateOption.AlternateSourceIP) != 0)
            {
                SourceIP = new System.Net.IPAddress(remainData.Take(4).ToArray());
                remainData = remainData.Skip(7).ToList();
            }

            if ((UpdateOption & ServerListUpdateOption.LimitResultCount) != 0)
            {
                int maxServers;
                if (!int.TryParse(remainData.Take(4).Reverse().ToString(), out maxServers))
                {
                    throw new SBException("max servers is not correct.");
                }
                MaxServers = maxServers;
            }
        }
    }
}
