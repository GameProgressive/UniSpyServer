using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
//last one we search with email this may get few profile so we can not return GPErrorCode
//SearchWithEmail(client,dict );
//\search\\sesskey\0\profileid\0\namespaceid\1\partnerid\0\nick\mycrysis\uniquenick\xiaojiuwo\email\koujiangheng@live.cn\gamename\gmtest\final\
//\bsrdone\more\<more>\final\
//string sendingbuffer = 
//"\\bsr\\1\\nick\\mycrysis\\uniquenick\\1\\namespaceid\\0\\firstname\\jiangheng\\lastname\\kou\\email\\koujiangheng@live.cn\\bsrdone\\0\\final\\";
//client.Stream.SendAsync(sendingbuffer);
//\more\<number of items>\final\
//\search\sesskey\0\profileid\0\namespaceid\0\nick\gbr359_jordips\gamename\gbrome\final\

namespace PresenceSearchPlayer.Handler.Search
{
    public class SearchHandler : GPSPHandlerBase
    {
        public SearchHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPSPSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("sesskey") && !_recv.ContainsKey("email") && !_recv.ContainsKey("uniquenick"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            //TODO verify the search condition whether needed namespaceid!!!!!


            //we only need uniquenick to search a profile
            if (_recv.ContainsKey("uniquenick"))
            {
                _result
                = SearchQuery.GetProfileFromUniquenick(_recv["uniquenick"], _namespaceid);
            }
            else if (_recv.ContainsKey("nick") && _recv.ContainsKey("email"))
            {
                _result = SearchQuery.GetProfileFromNickEmail(_recv["nick"], _recv["email"], _namespaceid);
            }
            else if (_recv.ContainsKey("nick"))
            {
                _result = SearchQuery.GetProfileFromNick(_recv["nick"], _namespaceid);
            }
            else if (_recv.ContainsKey("email"))
            {
                _result = SearchQuery.GetProfileFromEmail(_recv);
            }
            else
            {
                session.ToLog("Unknow search request received!");
                return;
            }
        }

        protected override void ConstructResponse(GPSPSession session)
        {
            int index;
            if (_result.Count == 1)
            {
                index = 0;
            }
            //We received request which needs more results.
            else
            {
                if (_recv.ContainsKey("skip"))
                {
                    index = Convert.ToInt16(_recv["skip"]);
                }
                else
                {
                    index = 0;
                }
            }
            if (index < _result.Count)
            {
                _sendingBuffer = @"\bsr\" + Convert.ToUInt16(_result[index]["profileid"]);
                _sendingBuffer += @"\nick\" + _result[index]["nick"];
                _sendingBuffer += @"\uniquenick\" + _result[index]["uniquenick"];
                _sendingBuffer += @"\namespaceid\" + Convert.ToUInt16(_result[index]["namespaceid"]);
                _sendingBuffer += @"\firstname\" + _result[index]["firstname"];
                _sendingBuffer += @"\lastname\" + _result[index]["lastname"];
                _sendingBuffer += @"\email\" + _result[index]["email"];
                _sendingBuffer += @"\bsrdone\\more\" + (_result.Count - 1) + @"\final\";
            }
        }
    }
}


