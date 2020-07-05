using System;
using System.Xml;
using System.Xml.Linq;
using RetroSpyServices.Sake.Entity.Interface;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Request;

namespace RetroSpyServices.Sake.Service
{
    public class SakeStorageService : ISakeStorageService
    {
        public void CreateRecord(SakeCreateRecordRequest request)
        {
            Console.WriteLine(request.ToString());
        }

        public void DeleteRecord(SakeDeleteRecordRequest request)
        {
            throw new NotImplementedException();
        }

        public void GetMyRecords(SakeGetMyRecordsRequest request)
        {
            throw new NotImplementedException();
        }

        public void GetRandomRecords(SakeGetRamdomRecordRequest request)
        {
            throw new NotImplementedException();
        }

        public void GetRecordLimit(SakeGetRecordLimitRecordRequest request)
        {
            throw new NotImplementedException();
        }

        public void GetSpecificRecords(SakeGetSpecificRecordsRequest request)
        {
            throw new NotImplementedException();
        }

        public void RateRecord(SakeRateRecordRequest request)
        {
            throw new NotImplementedException();
        }

        public void SearchForRecords(SakeSearchForRecordRequest request)
        {
            throw new NotImplementedException();
        }

        public void UpdateRecord(SakeUpdateRecordRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
