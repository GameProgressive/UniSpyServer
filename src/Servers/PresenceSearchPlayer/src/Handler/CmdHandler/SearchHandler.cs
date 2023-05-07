using System.Collections.Generic;
using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Request;
using UniSpy.Server.PresenceSearchPlayer.Contract.Response;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;
using System.Linq;
using UniSpy.Server.PresenceSearchPlayer.Application;


//last one we search with email this may get few profile so we can not return GPErrorCode
//SearchWithEmail(client,dict );
//\search\\sesskey\0\profileid\0\namespaceid\1\partnerid\0\nick\mycrysis\uniquenick\xiaojiuwo\email\koujiangheng@live.cn\gamename\gmtest\final\
//\bsrdone\more\<more>\final\
//string sendingbuffer = 
//"\\bsr\\1\\nick\\mycrysis\\uniquenick\\1\\namespaceid\\0\\firstname\\jiangheng\\lastname\\kou\\email\\koujiangheng@live.cn\\bsrdone\\0\\final\\";
//client.Stream.SendAsync(sendingbuffer);
//\more\<number of items>\final\
//\search\sesskey\0\profileid\0\namespaceid\0\nick\gbr359_jordips\gamename\gbrome\final\

namespace UniSpy.Server.PresenceSearchPlayer.Handler.CmdHandler
{

    public sealed class SearchHandler : CmdHandlerBase
    {
        private new SearchRequest _request => (SearchRequest)base._request;
        private new SearchResult _result { get => (SearchResult)base._result; set => base._result = value; }
        public SearchHandler(Client client, SearchRequest request) : base(client, request)
        {
            _result = new SearchResult();
        }
        protected override void DataOperation()
        {
            //TODO verify the search condition whether needed namespaceid!!!!!
            List<SearchDataBaseModel> result;
            switch (_request.RequestType)
            {
                case SearchRequestType.NickSearch:
                    result = StorageOperation.Persistance.GetMatchedInfosByNick(_request.Nick);
                    break;
                case SearchRequestType.NickEmailSearch:
                    result = StorageOperation.Persistance.GetMatchedInfosByNickAndEmail(_request.Nick,
                                                                                        _request.Email);
                    break;
                case SearchRequestType.UniquenickNamespaceIDSearch:
                    result = StorageOperation.Persistance.GetMatchedInfosByUniqueNickAndNamespaceId(_request.Uniquenick,
                                                                                                    _request.NamespaceID);
                    break;
                case SearchRequestType.EmailSearch:
                    result = StorageOperation.Persistance.GetMatchedInfosByEmail(_request.Email);
                    break;
                default:
                    result = null;
                    break;
            }
            if (result is null)
            {
                return;
            }
            _result.DataBaseResults = result.ToList();
        }

        protected override void ResponseConstruct()
        {
            _response = new SearchResponse(_request, _result);
        }
    }
}
