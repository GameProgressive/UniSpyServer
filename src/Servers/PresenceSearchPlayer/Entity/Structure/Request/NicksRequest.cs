using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.MiscMethod;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    internal class NicksRequest : PSPRequestBase
    {
        public NicksRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Password { get; private set; }
        public string Email { get; private set; }
        public uint NamespaceID { get; protected set; }
        public bool IsRequireUniqueNicks { get; protected set; }

        public override void Parse()
        {
            base.Parse();




            string md5Password;
            if (!PasswordEncoder.ProcessPassword(RequestKeyValues, out md5Password))
            {
                throw new GPGeneralException("password provided is invalid.", GPErrorCode.Parse);
            }
            Password = md5Password;


            if (!RequestKeyValues.ContainsKey("email"))
            {
                throw new GPGeneralException("email is missing.", GPErrorCode.Parse);
            }

            IsRequireUniqueNicks = true;

            if (RequestKeyValues.ContainsKey("pass"))
            {
                // Old games might send an error is unique nicknames are sent (like GSA 1.0)
                IsRequireUniqueNicks = false;
            }

            Email = RequestKeyValues["email"];

            if (RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPGeneralException("namespaceid is incorrect.", GPErrorCode.Parse);
                }
                NamespaceID = namespaceID;
            }
        }
    }
}
