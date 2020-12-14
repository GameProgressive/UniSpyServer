using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
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
    public enum ModeRequestType
    {
        GetChannelModes,

        EnableUserQuietFlag,
        DisableUserQuietFlag,

        AddChannelPassword,
        RemoveChannelPassword,

        AddChannelUserLimits,
        RemoveChannelUserLimits,

        AddBanOnUser,
        RemoveBanOnUser,

        AddChannelOperator,
        RemoveChannelOperator,

        EnableUserVoicePermission,
        DisableUserVoicePermission,

        SetChannelModes,
        SetChannelModesWithUserLimit,
    }

    public class MODERequest : ChatRequestBase
    {
        public MODERequest(string rawRequest) : base(rawRequest)
        {
        }

        public ModeRequestType RequestType { get; protected set; }
        public string ChannelName { get; protected set; }
        public string NickName { get; protected set; }
        public string UserName { get; protected set; }
        public uint LimitNumber { get; protected set; }
        public string ModeFlag { get; protected set; }
        public string Password { get; protected set; }


        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }


            if (_cmdParams.Count == 1)
            {
                ChannelName = _cmdParams[0];
                RequestType = ModeRequestType.GetChannelModes;
                ErrorCode = true;
            }

            if (_cmdParams.Count >= 2)
            {
                if (_cmdParams[1].Length <= 3)
                {
                    ModeFlag = _cmdParams[1];

                    switch (ModeFlag)
                    {
                        case "+q":
                            NickName = _cmdParams[0];
                            RequestType = ModeRequestType.EnableUserQuietFlag;
                            ErrorCode = true;
                            break;
                        case "-q":
                            NickName = _cmdParams[0];
                            RequestType = ModeRequestType.DisableUserQuietFlag;
                            ErrorCode = true;
                            break;
                        case "+k":
                            ChannelName = _cmdParams[0];
                            Password = _cmdParams[2];
                            RequestType = ModeRequestType.AddChannelPassword;
                            ErrorCode = true;
                            break;
                        case "-k":
                            ChannelName = _cmdParams[0];
                            Password = _cmdParams[2];
                            RequestType = ModeRequestType.RemoveChannelPassword;
                            ErrorCode = true;
                            break;
                        case "+l":
                            ChannelName = _cmdParams[0];
                            LimitNumber = uint.Parse(_cmdParams[2]);
                            RequestType = ModeRequestType.AddChannelUserLimits;
                            ErrorCode = true;
                            break;
                        case "-l":
                            ChannelName = _cmdParams[0];
                            RequestType = ModeRequestType.RemoveChannelUserLimits;
                            ErrorCode = true;
                            break;
                        case "+b":
                            ChannelName = _cmdParams[0];
                            NickName = _cmdParams[2];
                            RequestType = ModeRequestType.AddBanOnUser;
                            ErrorCode = true;
                            break;
                        case "-b":
                            ChannelName = _cmdParams[0];
                            RequestType = ModeRequestType.RemoveBanOnUser;
                            break;
                        case "+co":
                            ChannelName = _cmdParams[0];
                            UserName = _cmdParams[2];
                            RequestType = ModeRequestType.AddChannelOperator;
                            ErrorCode = true;
                            break;
                        case "-co":
                            ChannelName = _cmdParams[0];
                            UserName = _cmdParams[2];
                            RequestType = ModeRequestType.RemoveChannelOperator;
                            ErrorCode = true;
                            break;
                        case "+cv":
                            ChannelName = _cmdParams[0];
                            NickName = _cmdParams[2];
                            RequestType = ModeRequestType.EnableUserVoicePermission;
                            ErrorCode = true;
                            break;
                        case "-cv":
                            ChannelName = _cmdParams[0];
                            NickName = _cmdParams[2];
                            RequestType = ModeRequestType.DisableUserVoicePermission;
                            ErrorCode = true;
                            break;
                    }
                }
                else
                {
                    // "MODE <channel name> <mode flags> <limit number>"
                    if (_cmdParams.Count == 3)
                    {
                        ChannelName = _cmdParams[0];
                        ModeFlag = _cmdParams[1];
                        LimitNumber = uint.Parse(_cmdParams[2]);
                        RequestType = ModeRequestType.SetChannelModesWithUserLimit;

                        ErrorCode = true;
                    }
                    // "MODE <channel name> <mode flags>"
                    else
                    {
                        ChannelName = _cmdParams[0];
                        ModeFlag = _cmdParams[1];
                        RequestType = ModeRequestType.SetChannelModes;
                        ErrorCode = true;
                    }
                }
                ErrorCode = false;
                return;
            }
            ErrorCode = false;
        }
    }
}
