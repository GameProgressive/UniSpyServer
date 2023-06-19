using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UniSpy.Server.WebServer.Module.Sake.Contract.Request;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.WebServer.Module.Sake.Handler;
using UniSpy.Server.WebServer.Module.Auth.Handler;
using UniSpy.Server.WebServer.Module.Auth.Contract.Request;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Request;
using UniSpy.Server.WebServer.Module.Direct2Game.Handler;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Handler
{
    public class CmdSwitcher : CmdSwitcherBase
    {
        private new IHttpRequest _rawRequest => (IHttpRequest)base._rawRequest;
        private new Client _client => (Client)base._client;
        public CmdSwitcher(Client client, IHttpRequest rawRequest) : base(client, rawRequest)
        {
        }

        protected override void ProcessRawRequest()
        {
            // var uri = new Uri(_rawRequest.Url);
            // if (WebEndpoints.AvailableEndpoints.Contains(uri.LocalPath))
            // {
            //     throw new UniSpy.Exception($"Invalid http path access:{_rawRequest.Url}");
            // }
            try
            {
                dynamic xelements = XElement.Parse(_rawRequest.Body);
                var name = xelements.FirstNode.FirstNode.Name.LocalName;
                _requests.Add(new KeyValuePair<object, object>(name, _rawRequest.Body));
            }
            catch (System.Exception ex)
            {
                throw new WebServer.Exception("xml parsing error", ex);
            }
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            switch ((string)name)
            {
                #region Altas
                case "CreateMatchlessSession":
                case "CreateSession":
                case "SetReportIntention":
                case "SubmitReport":
                    throw new NotImplementedException();
                #endregion
                #region Auth
                case "LoginProfile":
                    return new LoginProfileHandler(_client, new LoginProfileRequest((string)rawRequest));
                case "LoginProfileWithGameId":
                    return new LoginProfileWithGameIdHandler(_client, new LoginProfileWithGameIdRequest((string)rawRequest));
                case "LoginRemoteAuth":
                    return new LoginRemoteAuthHandler(_client, new LoginRemoteAuthRequest((string)rawRequest));
                case "LoginRemoteAuthWithGameId":
                    return new LoginRemoteAuthWithGameIdHandler(_client, new LoginRemoteAuthWithGameIdRequest((string)rawRequest));
                case "LoginUniqueNick":
                    return new LoginUniqueNickHandler(_client, new LoginUniqueNickRequest((string)rawRequest));
                case "LoginUniqueNickWithGameId":
                    return new LoginUniqueNickWithGameIdHandler(_client, new LoginUniqueNickWithGameIdRequest((string)rawRequest));
                #endregion
                #region Direct2Game
                case "GetStoreAvailability":
                    return new GetStoreAvailabilityHandler(_client, new GetStoreAvailabilityRequest((string)rawRequest));

                case "GetPurchaseHistory":
                    return new GetPurchaseHistoryHandler(_client, new GetPurchaseHistoryRequest((string)rawRequest));
                case "GetTargettedAd":
                    throw new NotImplementedException();
                #endregion
                #region InGameAd
                case "ReportAdUsage":
                    throw new NotImplementedException();
                #endregion
                #region PatchingAndTracking
                case "Motd":
                case "Vercheck":
                    throw new NotImplementedException();
                #endregion
                #region Racing
                case "GetContestData":
                case "GetFriendRankings":
                case "GetRegionalData":
                case "GetTenAboveRankings":
                case "GetTopTenRankings":
                case "SubmitScores":
                    throw new NotImplementedException();
                #endregion
                #region Sake
                case "CreateRecord":
                    return new CreateRecordHandler(_client, new CreateRecordRequest((string)rawRequest));
                case "DeleteRecord":
                    throw new NotImplementedException();
                case "GetMyRecords":
                    return new GetMyRecordsHandler(_client, new GetMyRecordsRequest((string)rawRequest));
                case "GetRandomRecords":
                    throw new NotImplementedException();
                case "GetRecordLimit":
                    throw new NotImplementedException();
                case "RateRecord":
                    throw new NotImplementedException();
                case "SearchForRecords":
                    return new SearchForRecordsHandler(_client, new SearchForRecordsRequest((string)rawRequest));
                case "UpdateRecord":
                    // return new UpdateRecordHandler(_client, new UpdateRecordRequest((string)rawRequest));
                    throw new NotImplementedException();
                #endregion
                default:
                    _client.LogError($"Unkown {(string)name} request received");
                    return null;
            }
        }
    }
}