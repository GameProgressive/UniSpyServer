using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Extensions;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.Login.Misc;
using PresenceConnectionManager.Structure;

namespace PresenceConnectionManager.Handler.General.Login.LoginMethod
{

    public class LoginHandlerBase : GPCMHandlerBase
    {
        Crc16 _crc = new Crc16(Crc16Mode.Standard);

        protected LoginHandlerBase(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPCMSession session)
        {
            // Make sure we have all the required data to process this login
            if (!_recv.ContainsKey("challenge") || !_recv.ContainsKey("response"))
            {
                _errorCode = GPErrorCode.Parse;
                session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                return;
            }

            if(!IsChallengeCorrect(session))
            {
                //TODO check the challenge response correctness
                _errorCode = GPErrorCode.LoginBadLoginTicket;
                session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                return;
            }
        }

        protected bool IsChallengeCorrect(GPCMSession session)
        {
            string response = ChallengeProof.GenerateProof
                (
                session.PlayerInfo,
                session.PlayerInfo.UserChallenge,
                session.PlayerInfo.ServerChallenge,
                session.PlayerInfo.PasswordHash
                ) ;
            if (_recv["response"] == response)
            {
                return true;
            }
            return false;
        }




    }
}
