using System.ServiceModel;
using System.Xml;
using System.Xml.Linq;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model;

namespace RetroSpyServices.Sake.Entity.Interface
{
    [ServiceContract(Namespace = "http://gamespy.com/sake")]
    public interface ISakeStorageService
    {
        [OperationContract]
        public void DeleteRecord(XElement request);

        [OperationContract]
        public void GetRecordLimit(XElement request);

        [OperationContract]
        public void GetRandomRecords(XElement request);

        [OperationContract]
        public void GetMyRecords(XElement request);

        [OperationContract]
        public void SearchForRecords(XElement request);

        [OperationContract]
        public void GetSpecificRecords(XElement request);

        [OperationContract]
        public void CreateRecord(XElement request);

        [OperationContract]
        public void RateRecord(XElement request);

        [OperationContract]
        public void UpdateRecord(XElement request);
    }
}