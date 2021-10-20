﻿using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.PresenceConnectionManager.Handler.CmdHandler
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
    [HandlerContract("addbuddy")]
    public sealed class AddBuddyHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new AddBuddyRequest _request => (AddBuddyRequest)base._request;

        public AddBuddyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new AddBuddyResult();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
            //Check if the friend is online
            //if(online)
            //else
            //store add request to database
        }
    }
}
