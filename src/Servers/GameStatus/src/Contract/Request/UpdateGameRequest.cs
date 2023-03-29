using System.Linq;
using UniSpy.Server.Core.MiscMethod;
using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Exception;

namespace UniSpy.Server.GameStatus.Contract.Request
{

    public sealed class UpdateGameRequest : RequestBase
    {
        public int? ConnectionID { get; private set; }
        public bool IsDone { get; private set; }
        public bool IsClientLocalStorageAvailable { get; private set; }
        public string GameData { get; private set; }
        public int? SessionKey { get; private set; }
        public UpdateGameRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            CommandName = GameSpyUtils.GetRequestName(RawRequest);
            var index = RawRequest.IndexOf(@"gamedata\");
            GameData = RawRequest.Substring(index + @"gamedata\".Length).Replace(@"\final\", "");

            var defaultData = RawRequest.Substring(0, index);
            KeyValues = GameSpyUtils.ConvertToKeyValue(defaultData);
            KeyValues["gamedata"] = GameData;

            if (KeyValues.ContainsKey("dl"))
            {
                IsClientLocalStorageAvailable = true;
            }

            if (!KeyValues.ContainsKey("done"))
            {
                throw new GSException("done is missing.");
            }


            if (KeyValues["done"] == "1")
            {
                IsDone = true;
            }
            else if (KeyValues["done"] == "0")
            {
                IsDone = false;
            }
            else
            {
                throw new GSException("done format is incorrect.");
            }


            int sessKey;
            if (!int.TryParse(KeyValues["sesskey"], out sessKey))
            {
                throw new GSException("sesskey is not a valid int.");
            }
            SessionKey = sessKey;


            if (KeyValues.ContainsKey("connid"))
            {
                int connID;
                if (!int.TryParse(KeyValues["connid"], out connID))
                {
                    throw new GSException("connid is not a valid int.");
                }
                ConnectionID = connID;
            }
        }
    }
}