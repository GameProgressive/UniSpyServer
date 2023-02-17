using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Channel
{
    //request:
    //"MODE <nick> +/-q"

    //"MODE <channel name> +/-k <password>"

    // "MODE <channel name> +l <limit number>"
    // "MODE <channel name> -l"

    // "MODE <channel name> +b" actually we do not care about this request
    // "MODE <channel name> +/-b <nick name>"

    // "MODE <channel name> +/-co <user name>"
    // "MODE <channel name> +/-cv <user name>"

    // "MODE <channel name> <mode flags>"
    // "MODE <channel name> <mode flags> <limit number>"
    public enum ModeOperationType
    {
        EnableUserQuietFlag,
        DisableUserQuietFlag,
        AddChannelPassword,
        RemoveChannelPassword,
        AddChannelUserLimits,
        RemoveChannelUserLimits,
        AddBanOnUser,
        GetBannedUsers,
        RemoveBanOnUser,
        AddChannelOperator,
        RemoveChannelOperator,
        EnableUserVoicePermission,
        DisableUserVoicePermission,
        SetInvitedOnly,
        RemoveInvitedOnly,
        SetPrivateChannelFlag,
        RemovePrivateChannelFlag,
        SetSecretChannelFlag,
        RemoveSecretChannelFlag,
        SetModeratedChannelFlag,
        RemoveModeratedChannelFlag,
        EnableExternalMessagesFlag,
        DisableExternalMessagesFlag,
        SetTopicChangeByOperatorFlag,
        RemoveTopicChangeByOperatorFlag,
        SetOperatorAbeyChannelLimits,
        RemoveOperatorAbeyChannelLimits,
    }
    public enum ModeRequestType
    {
        GetChannelModes,
        GetChannelUserModes,
        SetChannelModes

    }
    
    public sealed class ModeRequest : ChannelRequestBase
    {
        public ModeRequestType RequestType { get; set; }
        public List<ModeOperationType> ModeOperations { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public int LimitNumber { get; set; }
        public string ModeFlag { get; set; }
        public string Password { get; set; }
        public ModeRequest(string rawRequest) : base(rawRequest)
        {
            ModeOperations = new List<ModeOperationType>();
        }
        public ModeRequest() { }

        public override void Parse()
        {
            base.Parse();
            if (_cmdParams.Count == 1)
            {
                RequestType = ModeRequestType.GetChannelModes;
            }
            else if (_cmdParams.Count == 2 || _cmdParams.Count == 3)
            {
                RequestType = ModeRequestType.SetChannelModes;
                ModeFlag = _cmdParams[1];
                var modeFlags = Regex.Split(ModeFlag, @"(?=\+|\-)").Where(s => !String.IsNullOrEmpty(s)).ToArray();
                foreach (var flag in modeFlags)
                {
                    switch (flag)
                    {
                        case "+e":
                            ModeOperations.Add(ModeOperationType.SetOperatorAbeyChannelLimits);
                            break;
                        case "-e":
                            ModeOperations.Add(ModeOperationType.RemoveOperatorAbeyChannelLimits);
                            break;
                        case "+t":
                            ModeOperations.Add(ModeOperationType.SetTopicChangeByOperatorFlag);
                            break;
                        case "-t":
                            ModeOperations.Add(ModeOperationType.RemoveTopicChangeByOperatorFlag);
                            break;
                        case "+n":
                            ModeOperations.Add(ModeOperationType.EnableExternalMessagesFlag);
                            break;
                        case "-n":
                            ModeOperations.Add(ModeOperationType.DisableExternalMessagesFlag);
                            break;
                        case "+m":
                            ModeOperations.Add(ModeOperationType.SetModeratedChannelFlag);
                            break;
                        case "-m":
                            ModeOperations.Add(ModeOperationType.RemoveModeratedChannelFlag);
                            break;
                        case "+s":
                            ModeOperations.Add(ModeOperationType.SetSecretChannelFlag);
                            break;
                        case "-s":
                            ModeOperations.Add(ModeOperationType.RemoveSecretChannelFlag);
                            break;
                        case "+i":
                            ModeOperations.Add(ModeOperationType.SetInvitedOnly);
                            break;
                        case "-i":
                            ModeOperations.Add(ModeOperationType.RemoveInvitedOnly);
                            break;
                        case "-p":
                            ModeOperations.Add(ModeOperationType.RemovePrivateChannelFlag);
                            break;
                        case "+p":
                            ModeOperations.Add(ModeOperationType.SetPrivateChannelFlag);
                            break;
                        case "+q":
                            NickName = _cmdParams[0];
                            ModeOperations.Add(ModeOperationType.EnableUserQuietFlag);
                            break;
                        case "-q":
                            NickName = _cmdParams[0];
                            ModeOperations.Add(ModeOperationType.DisableUserQuietFlag);
                            break;
                        case "+k":
                            ChannelName = _cmdParams[0];
                            Password = _cmdParams[2];
                            ModeOperations.Add(ModeOperationType.AddChannelPassword);
                            break;
                        case "-k":
                            ChannelName = _cmdParams[0];
                            Password = _cmdParams[2];
                            ModeOperations.Add(ModeOperationType.RemoveChannelPassword);
                            break;
                        case "+l":
                            ChannelName = _cmdParams[0];
                            LimitNumber = int.Parse(_cmdParams[2]);
                            ModeOperations.Add(ModeOperationType.AddChannelUserLimits);
                            break;
                        case "-l":
                            ChannelName = _cmdParams[0];
                            ModeOperations.Add(ModeOperationType.RemoveChannelUserLimits);
                            break;
                        case "+b":
                            ChannelName = _cmdParams[0];
                            if (_cmdParams.Count == 3)
                            {
                                NickName = _cmdParams[2];
                                ModeOperations.Add(ModeOperationType.AddBanOnUser);
                            }
                            else
                            {
                                ModeOperations.Add(ModeOperationType.GetBannedUsers);
                            }
                            break;
                        case "-b":
                            ChannelName = _cmdParams[0];
                            ModeOperations.Add(ModeOperationType.RemoveBanOnUser);
                            break;
                        case "+co":
                            ChannelName = _cmdParams[0];
                            UserName = _cmdParams[2];
                            ModeOperations.Add(ModeOperationType.AddChannelOperator);
                            break;
                        case "-co":
                            ChannelName = _cmdParams[0];
                            UserName = _cmdParams[2];
                            ModeOperations.Add(ModeOperationType.RemoveChannelOperator);
                            break;
                        case "+cv":
                            ChannelName = _cmdParams[0];
                            NickName = _cmdParams[2];
                            ModeOperations.Add(ModeOperationType.EnableUserVoicePermission);
                            break;
                        case "-cv":
                            ChannelName = _cmdParams[0];
                            NickName = _cmdParams[2];
                            ModeOperations.Add(ModeOperationType.DisableUserVoicePermission);
                            break;
                        default:
                            throw new Exception.ChatException("Unknown mode request type.");
                    }

                }
                // TODO! We ignore multiple mode flags, maybe we need it in future for supporting games

            }
            else
            {
                throw new Exception.ChatException("number of IRC parameters are incorrect.");
            }
        }
    }
}
