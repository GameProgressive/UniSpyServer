using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Extensions;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Group;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.SendGroups
{
    public class SendGroupsHandler : UpdateOptionHandlerBase
    {
        private PeerGroup _peerGroup;
        public SendGroupsHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
            _peerGroup = RetroSpyRedisExtensions.GetGroupsList<PeerGroup>(_request.GameName);
            if (_peerGroup == null||_peerGroup.PeerRooms.Count==0)
            {
                _errorCode = SBErrorCode.NoGroupRoomFound;
                return;
            }
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
            GenerateServerKeys();
            GenerateUniqueValue();
            GenerateServersInfo();
            _dataList.AddRange(SBStringFlag.AllServerEndFlag);
        }

        protected override void GenerateServerKeys()
        {
            //we add the total number of the requested keys
            _dataList.Add((byte)_request.FieldList.Length);
            //then we add the keys
            foreach (var key in _request.FieldList)
            {
                if (_peerGroup.PeerRooms[0].StandardKeyValue.ContainsKey(key))
                {
                    _dataList.Add((byte)SBKeyType.String);
                    _dataList.AddRange(Encoding.ASCII.GetBytes(key));
                    _dataList.Add(SBStringFlag.StringSpliter);
                }
            }
        }

        protected override void GenerateUniqueValue()
        {
            _dataList.Add(0);
        }

        protected override void GenerateServersInfo()
        {

            foreach (var room in _peerGroup.PeerRooms)
            {
                List<byte> header = new List<byte>();
                GenerateServerInfoHeader(header, null);
                _dataList.AddRange(header);
                foreach (var key in _request.FieldList)
                {
                    if (room.StandardKeyValue.ContainsKey(key))
                    {
                        _dataList.Add(SBStringFlag.NTSStringFlag);
                        _dataList.AddRange(Encoding.ASCII.GetBytes(room.StandardKeyValue[key]));
                        _dataList.Add(SBStringFlag.StringSpliter);
                    }
                }
            }
        }

        protected override void GenerateServerInfoHeader(List<byte> header,DedicatedGameServer server)
        {
            //add has key flag
            header.Add((byte)GameServerFlags.HasKeysFlag);
            //we add server public ip here
            header.AddRange(new byte[] { 0,0,0,0});
        }
    }
}
