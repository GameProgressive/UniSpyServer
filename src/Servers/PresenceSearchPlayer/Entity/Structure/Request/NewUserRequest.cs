using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
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
                throw new GPGeneralException("password provided is invalid.", GPErrorCode.Parse);
            }
            Password = md5Password;

            if (!RequestKeyValues.ContainsKey("nick"))
            {
                throw new GPGeneralException("nickname is missing.", GPErrorCode.Parse);
            }

            if (!RequestKeyValues.ContainsKey("email"))
            {
                throw new GPGeneralException("email is missing.", GPErrorCode.Parse);
            }
            if (!GameSpyUtils.IsEmailFormatCorrect(RequestKeyValues["email"]))
            {
                throw new GPGeneralException("email format is incorrect.", GPErrorCode.Parse);
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
                        throw new GPGeneralException("namespaceid is incorrect.", GPErrorCode.Parse);
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
                    throw new GPGeneralException("partnerid is incorrect.", GPErrorCode.Parse);
                }
                HasPartnerIDFlag = true;
                PartnerID = partnerid;
            }



            if (RequestKeyValues.ContainsKey("productid"))
            {
                uint productid;
                if (!uint.TryParse(RequestKeyValues["productid"], out productid))
                {
                    throw new GPGeneralException("productid is incorrect.", GPErrorCode.Parse);

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
                    throw new GPGeneralException("port is incorrect.", GPErrorCode.Parse);
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
