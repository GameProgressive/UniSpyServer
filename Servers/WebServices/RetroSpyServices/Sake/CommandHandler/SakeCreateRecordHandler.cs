using System;
using GameSpyLib.Common.Entity.Interface;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Request;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Response;

namespace WebServices.RetroSpyServices.Sake.CommandHandler
{
    public class SakeCreateRecordHandler:IHandler
    {
        SakeCreateRecordRequest _request;
        SakeCreateRecordResponse _response;
        public SakeCreateRecordHandler(SakeCreateRecordRequest request)
        {
            _request = request;
        }

        public IHandler Handle()
        {
            throw new NotImplementedException();
        }
    }
}
