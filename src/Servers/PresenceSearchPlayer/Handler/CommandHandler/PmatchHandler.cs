//using UniSpyLib.Common;
//using PresenceSearchPlayer.Enumerate;
//using System;
//using System.Collections.Generic;

//namespace PresenceSearchPlayer.Handler.CommandHandler.Pmatch
//{
//    /// <summary>
//    /// Search the all players in specific game
//    /// </summary>
//    public class PmatchHandler:GPSPHandlerBase
//    {

//        public PmatchHandler(string rawRequest) :base(rawRequest)
//        {
//        }

//            //pmath\\sesskey\\profileid\\productid\\

//        protected override void CheckRequest(GPSPSession session)
//        {
//            base.CheckRequest(session);
//            if (!KeyValues.ContainsKey("sesskey") || !KeyValues.ContainsKey("profileid") || !KeyValues.ContainsKey("productid"))
//            {
//                _errorCode = GPErrorCode.Parse;
//            }
//        }

//        protected override void DataBaseOperation(GPSPSession session)
//        {
//            _result = PmatchQuery.PlayerMatch(Convert.ToUInt16(KeyValues["productid"]));
//        }

//        protected override void ConstructResponse(GPSPSession session)
//        {
//            if(_result.Count==0)
//            {
//                _sendingBuffer= @"\psrdone\final\";
//                return;
//            }
//            foreach (Dictionary<string, object> player in _result)
//            {
//                _sendingBuffer += @"\psr\" + player["profileid"];
//                _sendingBuffer += @"\status\" + player["statstring"];
//                _sendingBuffer += @"\nick\" + player["nick"];
//                _sendingBuffer += @"\statuscode\" + player["status"];
//            }
//
//            _sendingBuffer += @"\psrdone\final\";
//        }
//    }
//}
