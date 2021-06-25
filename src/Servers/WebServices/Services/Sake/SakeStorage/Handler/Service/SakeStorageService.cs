using RetroSpyServices.Sake.Entity.Interface;
using System;
using System.ServiceModel;
using UniSpyWebService.Models.SakeStorageServer.Response;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Request;
using WebServices.RetroSpyServices.Sake.Entity.Structure.Response;
using WebServices.RetroSpyServices.Sake.Handler.CommandHandler;

namespace RetroSpyServices.Sake.Handler.Service
{
    [ServiceContract(Namespace = "http://gamespy.com/sake")]
    public class SakeStorageService /*: ISakeStorageService*/
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

        [OperationContract(Action = "SearchForRecords")]
        public SakeResponse SearchForRecords(uint gameid, string secretKey, string loginTicket, string tableid, string filter, string sort, uint offset, uint max, int surrounding, uint[] ownerids, bool cacheFlag, string[] fields)
        {
            return new SakeResponse
            {
                response = new Response
                {
                    result = new Result
                    {
                        Code = "Success"
                    },
                    values = new UniSpyWebService.Models.SakeStorageServer.Response.Values
                    {
                        values = new ArrayOfValues
                        {
                            RecordValue = new RecordValue[]
                            {
                                new RecordValue
                                {
                                    booleanValue = new BooleanValue
                                    {
                                        value = true
                                    }
                                },
                                new RecordValue
                                {
                                    floatValue = new FloatValue
                                    {
                                        value = 10.0f
                                    }
                                }
                            }
                        }
                    }
                }
            };
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
