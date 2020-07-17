using System;
using GameSpyLib.Common.Entity.Interface;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Request;

namespace WebServices.RetroSpyServices.Sake.CommandHandler
{
    public abstract class SakeCommandHandlerBase
    {
        SakeRequestBase _request;
        public SakeCommandHandlerBase(SakeRequestBase request)
        {
            _request = request;
        }
    }
}
