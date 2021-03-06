﻿using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler
{
    public class UniqueSearchHandler : PSPCommandHandlerBase
    {
        protected new UniqueSearchRequest _request;
        private bool _isUniquenickExist;

        public UniqueSearchHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (UniqueSearchRequest)request;
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             join n in db.Subprofiles on p.Profileid equals n.Profileid
                             where n.Uniquenick == _request.PreferredNick
                             && n.Namespaceid == _request.NamespaceID
                             //&& n.Gamename == _request.GameName
                             select p.Profileid;

                if (result.Count() == 0)
                {
                    _isUniquenickExist = false;
                }
            }
        }

        protected override void BuildNormalResponse()
        {
            if (!_isUniquenickExist)
            {
                _sendingBuffer = @"us\0\usdone\final";
            }
            else
            {
                _sendingBuffer = @"\us\1\nick\Choose another name\usdone\final\";
            }
        }
    }
}
