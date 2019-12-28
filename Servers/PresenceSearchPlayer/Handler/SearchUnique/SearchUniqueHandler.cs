using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.SearchUnique
{
    /// <summary>
    /// Search with uniquenick and namespace
    /// </summary>
    public class SearchUniqueHandler:GPSPHandlerBase
    {
        public SearchUniqueHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPSPSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("uniquenick") || !_recv.ContainsKey("namespace"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void ConstructResponse(GPSPSession session)
        {
            _sendingBuffer = @"\bsr\" + Convert.ToUInt32(_result[0]["profileid"]);
            _sendingBuffer += @"\nick\" + _result[0]["nick"];
            _sendingBuffer += @"\uniquenick\" + _result[0]["uniquenick"];
            _sendingBuffer += @"\namespaceid\" + Convert.ToUInt16(_result[0]["namespaceid"]);
            _sendingBuffer += @"\firstname\" + _result[0]["firstname"];
            _sendingBuffer += @"\lastname\" + _result[0]["lastname"];
            _sendingBuffer += @"\email\" + _result[0]["email"];
            _sendingBuffer += @"\bsrdone\\more\0" + @"\final\";
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            _result[0] = SearchUniqueQuery.GetProfileWithUniquenickAndNamespace(_recv["uniquenick"], Convert.ToUInt16(_recv["namespaceid"]));
        }
    }
}

