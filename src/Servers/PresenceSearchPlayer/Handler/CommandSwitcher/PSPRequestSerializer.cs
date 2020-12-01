using System;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;

namespace PresenceSearchPlayer.Handler.CommandSwitcher
{
    public class PSPRequestSerializer:RequestSerializerBase
    {
        protected new string _rawRequest;
        public PSPRequestSerializer(object rawRequest) : base(rawRequest)
        {
            _rawRequest = (string)rawRequest;
        }

        public override IRequest Serialize()
        {            // Read client message, and parse it into key value pairs
            string[] recieved = _rawRequest.TrimStart('\\').Split('\\');
            Dictionary<string, string> keyValue = GameSpyUtils.ConvertToKeyValue(recieved);
            switch (keyValue.Keys.First())
            {
                case PSPRequestName.Search:
                    return new SearchRequest(keyValue);
                case PSPRequestName.Valid:
                    return new SearchRequest(keyValue);
                case PSPRequestName.Nicks:
                    return new SearchRequest(keyValue);
                case PSPRequestName.PMatch:
                case PSPRequestName.Check:
                    return new SearchRequest(keyValue);
                case PSPRequestName.NewUser:
                    return new SearchRequest(keyValue);
                case PSPRequestName.SearchUnique:
                    return new SearchRequest(keyValue);
                case PSPRequestName.Others:
                    return new SearchRequest(keyValue);
                case PSPRequestName.OtherList:
                    return new SearchRequest(keyValue);
                case PSPRequestName.UniqueSearch:
                    return new SearchRequest(keyValue);
                default:
                    LogWriter.UnknownDataRecieved(_rawRequest);
                    return null;
            }
        }
    }
}
