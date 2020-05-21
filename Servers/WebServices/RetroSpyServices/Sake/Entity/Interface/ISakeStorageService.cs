using System.ServiceModel;
using System.Xml;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model;

namespace RetroSpyServices.Sake.Entity.Interface
{
    [ServiceContract]
    public interface ISakeStorageService
    {
        [OperationContract]
        public bool DeleteRecord(SakeDeleteModel request);

        [OperationContract]
        public void GetRecordLimit(XmlElement request);

        [OperationContract]
        public void GetRandomRecords(XmlElement request);

        [OperationContract]
        public void GetMyRecords(XmlElement request);

        [OperationContract]
        public void SearchForRecords(XmlElement request);

        [OperationContract]
        public void GetSpecificRecords(XmlElement request);

        [OperationContract]
        public void CreateRecord(XmlElement request);

        [OperationContract]
        public void RateRecord(XmlElement request);

        [OperationContract]
        public void UpdateRecord(XmlElement request);
    }
}