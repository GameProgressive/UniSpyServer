using System;
using GameSpyLib.Database.DatabaseModel.MySql;
using ServerBrowser.Entity.Structure.Packet.Request;
using System.Linq;
using ServerBrowser.Entity.Enumerator;
using GameSpyLib.Encryption;
using System.Net;
using System.Collections.Generic;
using System.Text;
using ServerBrowser.Entity.Structure;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.SendGroups
{
    /// <summary>
    /// Only sends keys and values
    /// there are no servers infomation
    /// </summary>
    public class SendGroupsHandler : CommandHandlerBase
    {
        private ServerListRequest _request = new ServerListRequest();
        private IQueryable<Grouplist> _gourpList;
        private string _secretKey;
        private byte[] _clientRemoteIP;
        private byte[] _clientRemotePort;
        public static readonly List<string> FieldList =
            new List<string> { "groupid", "hostname", "numplayers", "maxwaiting", "numwaiting", "numservers", "password", "other" };
        public SendGroupsHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);

            IPEndPoint remote = (IPEndPoint)session.Socket.RemoteEndPoint;
            _clientRemoteIP = remote.Address.GetAddressBytes();
            //TODO   //check what is the default port
            _clientRemotePort = BitConverter.GetBytes((ushort)(6500 & 0xFFFF));
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
            if (!GetGroupList() || !GetSecretKey())
            {
                _errorCode = SBErrorCode.DataOperation;
                return;
            }
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);

            List<byte> data = new List<byte>();


            GOAEncryption enc = new GOAEncryption(_secretKey, _request.Challenge, SBServer.ServerChallenge);
            //enc.Encrypt();

        }
        public List<byte> GenerateServerInfo()
        {
            List<byte> data = new List<byte>();

            foreach (var room in _gourpList)
            {
                data.Add((byte)GameServerFlags.HasKeysFlag);
                data.AddRange(new byte[] { 192, 168, 0, 1 });
                data.Add(SBStringFlag.NTSStringFlag);
                data.AddRange(Encoding.ASCII.GetBytes(room.Name));
                data.Add(SBStringFlag.StringSpliter);

                data.Add(SBStringFlag.NTSStringFlag);
                data.AddRange(Encoding.ASCII.GetBytes(room.Numwaiting.ToString()));
                data.Add(SBStringFlag.StringSpliter);

                data.Add(SBStringFlag.NTSStringFlag);
                data.AddRange(Encoding.ASCII.GetBytes(room.Maxwaiting.ToString()));
                data.Add(SBStringFlag.StringSpliter);

                data.Add(SBStringFlag.NTSStringFlag);
                data.AddRange(Encoding.ASCII.GetBytes(room.Name));
                data.Add(SBStringFlag.StringSpliter);

                data.Add(SBStringFlag.NTSStringFlag);
                data.AddRange(Encoding.ASCII.GetBytes(room.Numservers.ToString()));
                data.Add(SBStringFlag.StringSpliter);

                data.Add(SBStringFlag.NTSStringFlag);
                data.AddRange(Encoding.ASCII.GetBytes(room.Numplayers.ToString()));
                data.Add(SBStringFlag.StringSpliter);
            }
            return data;
        }
        public List<byte> GenerateGroupKeys()
        {
            List<byte> data = new List<byte>();
            foreach (var key in _request.FieldList)
            {
                data.Add((byte)SBKeyType.String);
                data.AddRange(Encoding.ASCII.GetBytes(key));
                data.Add(SBStringFlag.StringSpliter);
            }
            return data;
        }

        public List<byte> GenerateGroupInfos()
        {
            return null;
        }

        private bool GetGroupList()
        {
            using (var db = new retrospyContext())
            {
                IQueryable<Grouplist> result = from g in db.Grouplist
                                               join gn in db.Games on g.Gameid equals gn.Id
                                               where gn.Gamename == _request.GameName
                                               select g;
                if (result.Count() == 0)
                {
                    return false;
                }
                _gourpList = result;
                return true;
            }
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
