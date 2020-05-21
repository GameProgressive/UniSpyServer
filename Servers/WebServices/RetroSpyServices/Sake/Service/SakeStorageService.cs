using System;
using System.Xml;
using System.Xml.Linq;
using RetroSpyServices.Sake.Entity.Interface;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model;

namespace RetroSpyServices.Sake.Service
{
    public class SakeStorageService : ISakeStorageService
    {
        public void CreateRecord(XElement request)
        {
            Console.WriteLine(request.ToString());
        }

        public void DeleteRecord(XElement request)
        {
            throw new NotImplementedException();
        }

        public void GetMyRecords(XElement request)
        {
            throw new NotImplementedException();
        }

        public void GetRandomRecords(XElement request)
        {
            throw new NotImplementedException();
        }

        public void GetRecordLimit(SakeGetRecordLimitModel request)
        {
            throw new NotImplementedException();
        }

        public void GetSpecificRecords(XElement request)
        {
            throw new NotImplementedException();
        }

        public void RateRecord(XElement request)
        {
            throw new NotImplementedException();
        }

        public void SearchForRecords(XElement request)
        {
            throw new NotImplementedException();
        }

        public void UpdateRecord(XElement request)
        {
            throw new NotImplementedException();
        }
    }
}
