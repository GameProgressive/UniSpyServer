using System;
using System.Collections.Generic;
using GameSpyLib.Common;
using GameSpyLib.Extensions;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.Login.Misc;
using PresenceConnectionManager.Handler.General.SDKExtendFeature;

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
            // for pass operation id to session,Playerinfo
            base.CheckRequest(session);

            // Make sure we have all the required data to process this login
            if (!_recv.ContainsKey("challenge") || !_recv.ContainsKey("response"))
            {
                _errorCode = GPErrorCode.Parse;
                session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                return;
            }
            if (_recv.ContainsKey("partnerid"))
            {
                if (uint.TryParse(_recv["partnerid"], out session.PlayerInfo.PartnerID))
                {
                    _errorCode = GPErrorCode.Parse;
                    session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                }
            }
            if (!_recv.ContainsKey("passenc"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            session.PlayerInfo.UserChallenge = _recv["challenge"];
            session.PlayerInfo.PasswordHash = _recv["passenc"];

            if (!IsChallengeCorrect(session))
            {
                //TODO check the challenge response correctness
                _errorCode = GPErrorCode.LoginBadLoginTicket;
                session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                return;
            }



            if (_recv.ContainsKey("partnerid"))
            {
                // the default partnerid will be (uint)gamespy.
                session.PlayerInfo.PartnerID = Convert.ToUInt32(_recv["partnerid"]);
            }
            if (_recv.ContainsKey("namespaceid"))
            {
                // the default namespaceid = 0
                session.PlayerInfo.NamespaceID = Convert.ToUInt32(_recv["namespaceid"]);
            }
            //store sdkrevision
            if (_recv.ContainsKey("sdkrevision"))
            {
                session.PlayerInfo.SDKRevision = Convert.ToUInt32(_recv["sdkrevision"]);
            }

        }

        protected override void ConstructResponse(GPCMSession session)
        {
            string responseProof = ChallengeProof.GenerateProof
                (
                session.PlayerInfo,
                session.PlayerInfo.UserData,
                session.PlayerInfo.LoginType,
                session.PlayerInfo.PartnerID,
                session.PlayerInfo.ServerChallenge,
                session.PlayerInfo.UserChallenge,
                _result[0]["password"].ToString()
                );

            //string random = GameSpyRandom.GenerateRandomString(22, GameSpyRandom.StringType.Hex);
            //// Password is correct
            //_sendingBuffer = string.Format(
            //    @"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
            //    session.PlayerInfo.SessionKey,
            //    responseProof,
            //    _result[0]["profileid"],
            //    _result[0]["uniquenick"],
            //    // Generate LT whatever that is (some sort of random string, 22 chars long)
            //    random
            //    );

            _sendingBuffer = @"\lc\2\sessionkey\" + session.PlayerInfo.SessionKey;
            _sendingBuffer += @"\proof\" + responseProof;
            _sendingBuffer += @"\userid\" +_result[0]["profileid"];
            _sendingBuffer += @"\uniquenick\" + _result[0]["uniquenick"];
            _sendingBuffer += @"\lt\" + GameSpyRandom.GenerateRandomString(22, GameSpyRandom.StringType.Hex)+ @"__";
            _sendingBuffer += @"id" + session.PlayerInfo.OperationID;

            session.PlayerInfo.LoginProcess = LoginStatus.Completed;
        }

        protected bool IsChallengeCorrect(GPCMSession session)
        {
            string response = ChallengeProof.GenerateProof
                (
                session.PlayerInfo,
                session.PlayerInfo.UserData,
                session.PlayerInfo.LoginType,
                session.PlayerInfo.PartnerID,
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

        protected override void DataBaseOperation(GPCMSession session)
        {
            if(_result==null)
            {
                _errorCode = GPErrorCode.DatabaseError;
                return;
            }
            bool isVerified = Convert.ToBoolean(_result[0]["emailverified"]);
            bool isBanned = Convert.ToBoolean(_result[0]["banned"]);
            if (!isVerified)
            {
                session.PlayerInfo.DisconReason = DisconnectReason.InvalidPlayer;
                _errorCode = GPErrorCode.LoginBadEmail;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (isBanned)
            {
                session.PlayerInfo.DisconReason = DisconnectReason.PlayerIsBanned;
                _errorCode = GPErrorCode.LoginProfileDeleted;
            }
        }

        protected override void Response(GPCMSession session)
        {
            base.Response(session);
            SDKRevision.Switch(session, _recv);
        }
    }
}
