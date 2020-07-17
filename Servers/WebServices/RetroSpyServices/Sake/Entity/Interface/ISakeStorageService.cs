using System.ServiceModel;
using WebServices.RetroSpyServices.Sake.Entity.Structure;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Request;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Response;

namespace RetroSpyServices.Sake.Entity.Interface
{
    [ServiceContract(Namespace = SakeXmlLable.SakeNameSpace)]
    public interface ISakeStorageService
    {
        [OperationContract]
        public SakeDeleteRecordResponse DeleteRecord(SakeDeleteRecordRequest request);

        [OperationContract]
        public SakeGetRecordLimitReponse GetRecordLimit(SakeGetRecordLimitRequest request);

        [OperationContract]
        public SakeGetRandomRecordResponse GetRandomRecords(SakeGetRamdomRecordRequest request);

        [OperationContract]
        public SakeGetMyRecordsResponse GetMyRecords(SakeGetMyRecordsRequest request);

        [OperationContract]
        public SakeSearchForRecordResponse SearchForRecords(SakeSearchForRecordRequest request);

        [OperationContract]
        public SakeGetSpecificRecordsResponse GetSpecificRecords(SakeGetSpecificRecordsRequest request);

        [OperationContract]
        public SakeCreateRecordResponse CreateRecord(SakeCreateRecordRequest request);

        [OperationContract]
        public SakeRateRecordResponse RateRecord(SakeRateRecordRequest request);

        [OperationContract]
        public SakeUpdateRecordResponse UpdateRecord(SakeUpdateRecordRequest request);
    }
}