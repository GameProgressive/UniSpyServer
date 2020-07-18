using System.Collections.Generic;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Handler.CommandHandler.Check
{
    public class CheckRequestModel:RequestModelBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\

        public CheckRequestModel(Dictionary<string, string> recv) : base(recv)
        {
        }
      
        public override bool Parse(out GPErrorCode errorCode)
        {
            if (!base.Parse(out errorCode))
                return false;

            if (!_recv.ContainsKey("nick") || !_recv.ContainsKey("email") || !_recv.ContainsKey("passenc"))
            {
                errorCode = GPErrorCode.Parse;
                return false;
            }

            if (!GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                errorCode = GPErrorCode.CheckBadMail;
                return false;
            }

            return true;
        }
    }
}
