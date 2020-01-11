using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler.UniqueSearch
{
    public class UniqueSearchHandler:GPSPHandlerBase
    {
        public  UniqueSearchHandler(Dictionary<string, string> recv) : base(recv)
        {
        }
        bool IsUniquenickExist;
        uint namespaceid;       
        
        protected override void CheckRequest(GPSPSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("preferrednick"))
            {
                _errorCode = GPErrorCode.Parse;
            }
                

            if (_recv.ContainsKey("namespaceid"))
            {
               if( !uint.TryParse(_recv["namespaceid"], out namespaceid))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            IsUniquenickExist = UniqueSearchQuery.IsUniqueNickExist(_recv["preferrednick"], namespaceid);

        }
        protected override void ConstructResponse(GPSPSession session)
        {
            if (!IsUniquenickExist)
            {
                _sendingBuffer = @"us\0\usdone\final";
            }
            else
            {
                _sendingBuffer = @"\us\1\nick\" + _recv["preferrednick"] + @"\usdone\final\";
            }
            
        }
    }
}
