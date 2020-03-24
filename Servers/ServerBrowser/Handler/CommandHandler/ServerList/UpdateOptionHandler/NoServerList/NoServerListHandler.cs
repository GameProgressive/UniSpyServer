using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using GameSpyLib.Database.DatabaseModel.MySql;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Entity.Structure;
using System.Text;
using GameSpyLib.Encryption;
using ServerBrowser.Entity.Structure.Packet.Response;
using ServerBrowser.Handler.CommandHandler.ServerList.GetServers;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.NoServerList
{
    /// <summary>
    /// Send servers list without keys and values
    /// </summary>
    public class NoServerListHandler : CommandHandlerBase
    {
        private string _secretKey;
        private byte[] _clientRemoteIP;
        private byte[] _clientRemotePort;
        private ServerListRequest _request = new ServerListRequest();
        private IEnumerable<KeyValuePair<EndPoint, GameServer>> _filteredServers;

        public NoServerListHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);

            _request.Parse(recv);

            //this is client public ip and port
            IPEndPoint remote = (IPEndPoint)session.Socket.RemoteEndPoint;
            _clientRemoteIP = remote.Address.GetAddressBytes();
            //TODO   //check what is the default port
            _clientRemotePort = BitConverter.GetBytes((ushort)(6500 & 0xFFFF));
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
            _filteredServers = GetServersFromQR.GetFilteredServer(new GetServersFromMemory(), _request.GameName, _request.Filter);

            if (_filteredServers.Count() == 0)
            {
                _errorCode = SBErrorCode.NoServersFound;
            }
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
            List<byte> data = new List<byte>();

            //first add client public ip and port
            data.AddRange(_clientRemoteIP);
            data.AddRange(_clientRemotePort);
            //we add the number of keys
            data.Add(0);
            //we add the number of unique values
            data.Add(0);
            //there is not server info here

            //after all server information is added we add the end flag
            data.AddRange(SBStringFlag.AllServerEndFlag);

            //we get secrete key from database
            if (!GetSecretKey())
            {
                session.ToLog($"Unknown or unsupported game: {_request.GameName}");
                return;
            }

            session.ToLog($"[Plaintext] {Encoding.ASCII.GetString(data.ToArray())}");
            GOAEncryption enc
                = new GOAEncryption(_secretKey, _request.Challenge, SBServer.ServerChallenge);

            _sendingBuffer = new ServerListResponse().
                CombineHeaderAndContext
                (
                    enc.Encrypt(data.ToArray()),
                    SBServer.ServerChallenge
                ); ;

            //refresh encryption state
            session.EncState = enc.State;
        }

        private bool GetSecretKey()
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Games
                             where p.Gamename == _request.GameName
                             select new { p.Secretkey };

                if (result.Count() == 1)
                {
                    _secretKey = result.First().Secretkey;
                    return true;
                }
                else
                {
                    _errorCode = SBErrorCode.DataOperation;
                    return false;
                }
            }
        }

    }
}
