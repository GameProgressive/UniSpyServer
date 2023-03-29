using UniSpy.Server.Core.MiscMethod;
using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Exception;

namespace UniSpy.Server.GameStatus.Contract.Request
{
    // "\newgame\\sesskey\%d\challenge\%d";
    //"\newgame\\connid\%d\sesskey\%d"
    // worm3d \newgame\\connid\0\sesskey\87563063\final\
    public sealed class NewGameRequest : RequestBase
    {
        public bool IsClientLocalStorageAvailable { get; private set; }
        public string Challenge { get; private set; }
        public int? ConnectionID { get; private set; }
        /// <summary>
        /// new game data storage session key, this is different from authgame session key
        /// </summary>
        public int? SessionKey { get; private set; }
        public NewGameRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            CommandName = GameSpyUtils.GetRequestName(RawRequest);
            KeyValues = GameSpyUtils.ConvertToKeyValue(RawRequest);

            if (!KeyValues.ContainsKey("sesskey"))
            {
                throw new GSException("sesskey is missing.");
            }
            if (!int.TryParse(KeyValues["sesskey"], out var sessKey))
            {
                throw new GSException("sesskey is not a valid int.");
            }
            SessionKey = sessKey;

            if (!KeyValues.ContainsKey("connid"))
            {
                throw new GSException("connid is missing.");
            }
            if (!int.TryParse(KeyValues["connid"], out var connectionID))
            {
                throw new GSException("connid format is incorrect.");
            }
            ConnectionID = connectionID;

            if (KeyValues.ContainsKey("challenge"))
            {
                IsClientLocalStorageAvailable = true;
                Challenge = KeyValues["challenge"];
            }
        }
    }
}
