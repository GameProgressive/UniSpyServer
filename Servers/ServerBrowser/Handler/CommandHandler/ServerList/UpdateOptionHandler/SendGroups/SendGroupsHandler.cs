using System;
using System.Collections.Generic;
using GameSpyLib.Extensions;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Group;

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
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
        }

        protected override void GenerateServerKeys()
        {
            throw new NotImplementedException();
        }

        protected override void GenerateUniqueValue()
        {
            throw new NotImplementedException();
        }

        protected override void GenerateServersInfo()
        {
            throw new NotImplementedException();
        }

        protected override void GenerateServerInfoHeader(DedicatedGameServer server)
        {
            throw new NotImplementedException();
        }
    }
}
