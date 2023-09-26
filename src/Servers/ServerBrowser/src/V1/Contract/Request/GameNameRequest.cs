using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Contract.Request
{
    public enum EncryptionType
    {
        // do not encrypt
        Plaintext,
        // use encryption method 1
        Type1,
        // use encryption method 2
        Type2
    }
    /// <summary>
    /// verify client
    /// The request maybe \gamename\<gamename>\gamever\<game version>\location\0\validate\\secure\<encryption key>\final\\queryid\1.1\"
    /// </summary>
    public sealed class GameNameRequest : RequestBase
    {
        public EncryptionType EncType { get; private set; }
        public string Version { get; private set; }
        public string ValidateKey { get; private set; }
        public GameNameRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (KeyValues.ContainsKey("enctype"))
            {
                if (!System.Enum.TryParse<EncryptionType>(KeyValues["enctype"], out var type))
                {
                    throw new ServerBrowser.Exception("Encryption type format is not correct.");
                }
                EncType = type;
            }

            if (!KeyValues.ContainsKey("gamever"))
            {
                throw new ServerBrowser.Exception("Game engine version is not presented in request.");
            }

            Version = KeyValues["gamever"];
            //process secure
            // star trek armada 2 do not use encryption
            if (KeyValues.ContainsKey("secure"))
            {
                ValidateKey = KeyValues["secure"];
            }
        }
    }
}