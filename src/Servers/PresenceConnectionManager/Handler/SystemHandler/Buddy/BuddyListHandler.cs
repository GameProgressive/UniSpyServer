﻿using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Abstraction.SystemHandler
{
    public class BuddyListHandler : PCMCommandHandlerBase
    {
        protected List<uint> _profileIDList;
        public BuddyListHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            // \bdy\< num in list >\list\< profileid list - comma delimited >\final\
            _sendingBuffer = $@"\bdy\{_profileIDList.Count()}\list\";
            foreach (var pid in _profileIDList)
            {
                _sendingBuffer += $@"{pid}";
                if (pid != _profileIDList.Last())
                {
                    _sendingBuffer += ",";
                }
            }
            _sendingBuffer += @"\final\";
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            using (var db = new retrospyContext())
            {
                _profileIDList = db.Friends
                    .Where(f => f.Profileid == _session.UserData.ProfileID
                    && f.Namespaceid == _session.UserData.NamespaceID)
                    .Select(f => f.Targetid).ToList();
            }
        }

    }
}
