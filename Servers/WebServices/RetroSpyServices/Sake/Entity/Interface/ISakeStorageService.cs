using System.ServiceModel;
using WebServices.RetroSpyServices.Sake.Entity.Structure;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Request;

namespace RetroSpyServices.Sake.Entity.Interface
{
    [ServiceContract(Namespace = SakeXmlLable.SakeNameSpace)]
    public interface ISakeStorageService
    {
        [OperationContract]
        public void DeleteRecord(SakeDeleteRecordRequest request);

        [OperationContract]
        public void GetRecordLimit(SakeGetRecordLimitRecordRequest request);

        [OperationContract]
        public void GetRandomRecords(SakeGetRamdomRecordRequest request);

        [OperationContract]
        public void GetMyRecords(SakeGetMyRecordsRequest request);

        [OperationContract]
        public void SearchForRecords(SakeSearchForRecordRequest request);

        [OperationContract]
        public void GetSpecificRecords(SakeGetSpecificRecordsRequest request);

        [OperationContract]
        public void CreateRecord(SakeCreateRecordRequest request);

        [OperationContract]
        public void RateRecord(SakeRateRecordRequest request);

        [OperationContract]
        public void UpdateRecord(SakeUpdateRecordRequest request);
    }
}