using System;
using RetroSpyServices.Sake.Entity.Interface;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Request;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Response;
using WebServices.RetroSpyServices.Sake.Handler.CommandHandler;

namespace RetroSpyServices.Sake.Handler.Service
{
    public class SakeStorageService : ISakeStorageService
    {
        public SakeCreateRecordResponse CreateRecord(SakeCreateRecordRequest request)
        {
            CreateRecordHandler createRecord = new CreateRecordHandler(request);
            return (SakeCreateRecordResponse)createRecord.Handle();
        }

        public SakeDeleteRecordResponse DeleteRecord(SakeDeleteRecordRequest request)
        {
            throw new NotImplementedException();
        }

        public SakeGetMyRecordsResponse GetMyRecords(SakeGetMyRecordsRequest request)
        {
            throw new NotImplementedException();
        }

        public SakeGetRecordLimitReponse GetRecordLimit(SakeGetRecordLimitRequest request)
        {
            throw new NotImplementedException();
        }

        public SakeGetSpecificRecordsResponse GetSpecificRecords(SakeGetSpecificRecordsRequest request)
        {
            throw new NotImplementedException();
        }

        public SakeRateRecordResponse RateRecord(SakeRateRecordRequest request)
        {
            throw new NotImplementedException();
        }

        public SakeSearchForRecordResponse SearchForRecords(SakeSearchForRecordRequest request)
        {
            throw new NotImplementedException();
        }

        public SakeUpdateRecordResponse UpdateRecord(SakeUpdateRecordRequest request)
        {
            throw new NotImplementedException();
        }

        public SakeGetRandomRecordResponse GetRandomRecords(SakeGetRamdomRecordRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
