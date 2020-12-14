using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class NewProfileRequest : PCMRequestBase
    {
        //create a new profile with new nick 
        // @"  \newprofile\sesskey\<>\nick\<>\id\1\final\"
        //replace a existed nick with new nick
        //@"  \newprofile\sesskey\<>\nick\<>\replace\1\oldnick\<>\id\1\final\"

        public NewProfileRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string OldNick { get; protected set; }
        public string NewNick { get; protected set; }
        public bool IsReplaceNickName { get; protected set; }
        public override void Parse()
        {
            base.Parse();
            if( ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (KeyValues.ContainsKey("replace"))
            {
                if (!KeyValues.ContainsKey("oldnick") && !KeyValues.ContainsKey("nick"))
                {
                    ErrorCode = GPErrorCode.Parse; return;
                }

                OldNick = KeyValues["oldnick"];
                NewNick = KeyValues["nick"];
                IsReplaceNickName = true;
            }
            else
            {
                if (!KeyValues.ContainsKey("nick"))
                {
                    ErrorCode = GPErrorCode.Parse; return;
                }
                NewNick = KeyValues["nick"];
                IsReplaceNickName = false;
            }

            ErrorCode = GPErrorCode.NoError;
        }
    }
}
