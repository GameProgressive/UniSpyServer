using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;
using UniSpyLib.MiscMethod;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class NewUserRequest : PSPRequestBase
    {
        public uint ProductID { get; protected set; }
        public uint GamePort { get; protected set; }
        public string CDKeyEnc { get; protected set; }
        public bool HasGameNameFlag { get; protected set; }
        public bool HasProductIDFlag { get; protected set; }
        public bool HasCDKeyEncFlag { get; protected set; }
        public bool HasPartnerIDFlag { get; protected set; }
        public bool HasGamePortFlag { get; protected set; }

        public string Nick { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Uniquenick { get; private set; }
        public uint PartnerID { get; private set; }
        public string GameName { get; private set; }
        public uint NamespaceID { get; protected set; }
        public NewUserRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            string md5Password;
            if (!PasswordEncoder.ProcessPassword(RequestKeyValues, out md5Password))
            {
                throw new GPParseException("password provided is invalid.");
            }
            Password = md5Password;

            if (!RequestKeyValues.ContainsKey("nick"))
            {
                throw new GPParseException("nickname is missing.");
            }

            if (!RequestKeyValues.ContainsKey("email"))
            {
                throw new GPParseException("email is missing.");
            }
            if (!GameSpyUtils.IsEmailFormatCorrect(RequestKeyValues["email"]))
            {
                throw new GPParseException("email format is incorrect.");
            }

            Nick = RequestKeyValues["nick"];
            Email = RequestKeyValues["email"];

            if (RequestKeyValues.ContainsKey("uniquenick") && RequestKeyValues.ContainsKey("namespaceid"))
            {
                if (RequestKeyValues.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                    {
                        throw new GPParseException("namespaceid is incorrect.");
                    }

                    NamespaceID = namespaceID;
                }
                Uniquenick = RequestKeyValues["uniquenick"];
            }
            ParseOtherInfo();
        }

        private void ParseOtherInfo()
        {

            //parse other info
            if (RequestKeyValues.ContainsKey("partnerid"))
            {
                uint partnerid;
                if (!uint.TryParse(RequestKeyValues["partnerid"], out partnerid))
                {
                    throw new GPParseException("partnerid is incorrect.");
                }
                HasPartnerIDFlag = true;
                PartnerID = partnerid;
            }



            if (RequestKeyValues.ContainsKey("productid"))
            {
                uint productid;
                if (!uint.TryParse(RequestKeyValues["productid"], out productid))
                {
                    throw new GPParseException("productid is incorrect.");

                }
                HasProductIDFlag = true;
                ProductID = productid;
            }

            if (RequestKeyValues.ContainsKey("gamename"))
            {
                HasGameNameFlag = true;
                GameName = RequestKeyValues["gamename"];
            }


            if (RequestKeyValues.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(RequestKeyValues["port"], out port))
                {
                    throw new GPParseException("port is incorrect.");
                }
                HasGamePortFlag = true;
                GamePort = port;
            }

            if (RequestKeyValues.ContainsKey("cdkeyenc"))
            {
                HasCDKeyEncFlag = true;
                CDKeyEnc = RequestKeyValues["cdkeyenc"];
            }
        }
    }
}
