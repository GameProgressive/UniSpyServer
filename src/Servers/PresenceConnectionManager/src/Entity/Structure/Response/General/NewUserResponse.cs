﻿using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Response
{
    public sealed class NewUserResponse : ResponseBase
    {
        private new NewUserResult _result => (NewUserResult)base._result;
        public NewUserResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\nur\\userid\{_result.User.Userid}\profileid\{_result.SubProfile.Profileid}\id\{_request.OperationID}\final\";
        }
    }
}