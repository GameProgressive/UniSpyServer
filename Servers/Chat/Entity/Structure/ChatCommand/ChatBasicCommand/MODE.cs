using System;
using System.Collections.Generic;

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

    public class MODE : ChatCommandBase
    {
        public ModeRequestType RequestType { get; protected set; }
        public string ChannelName { get; protected set; }
        public string NickName { get; protected set; }
        public string UserName { get; protected set; }
        public uint LimitNumber { get; protected set; }
        public string ModeFlag { get; protected set; }
        public string Password { get; protected set; }

        public MODE()
        {
        }

        public override bool Parse(string message)
        {
            if (!base.Parse(message))
            {
                return false;
            }

            if (_cmdParams.Count == 1)
            {
                ChannelName = _cmdParams[0];
                RequestType = ModeRequestType.GetChannelModes;
                return true;
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
                            return true;
                        case "-q":
                            NickName = _cmdParams[0];
                            RequestType = ModeRequestType.DisableUserQuietFlag;
                            return true;
                        case "+k":
                            ChannelName = _cmdParams[0];
                            Password = _cmdParams[2];
                            RequestType = ModeRequestType.AddChannelPassword;
                            return true;
                        case "-k":
                            ChannelName = _cmdParams[0];
                            Password = _cmdParams[2];
                            RequestType = ModeRequestType.RemoveChannelPassword;
                            return true;
                        case "+l":
                            ChannelName = _cmdParams[0];
                            LimitNumber = uint.Parse(_cmdParams[2]);
                            RequestType = ModeRequestType.AddChannelUserLimits;
                            return true;
                        case "-l":
                            ChannelName = _cmdParams[0];
                            RequestType = ModeRequestType.RemoveChannelUserLimits;
                            return true;
                        case "+b":
                            ChannelName = _cmdParams[0];
                            NickName = _cmdParams[2];
                            RequestType = ModeRequestType.AddBanOnUser;
                            return true;
                        case "-b":
                            ChannelName = _cmdParams[0];
                            RequestType = ModeRequestType.RemoveBanOnUser;
                            break;
                        case "+co":
                            ChannelName = _cmdParams[0];
                            UserName = _cmdParams[2];
                            RequestType = ModeRequestType.AddChannelOperator;
                            return true;
                        case "-co":
                            ChannelName = _cmdParams[0];
                            UserName = _cmdParams[2];
                            RequestType = ModeRequestType.RemoveChannelOperator;
                            return true;
                        case "+cv":
                            ChannelName = _cmdParams[0];
                            NickName = _cmdParams[2];
                            RequestType = ModeRequestType.EnableUserVoicePermission;
                            return true;
                        case "-cv":
                            ChannelName = _cmdParams[0];
                            NickName = _cmdParams[2];
                            RequestType = ModeRequestType.DisableUserVoicePermission;
                            return true;
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
                      
                        return true;
                    }
                    // "MODE <channel name> <mode flags>"
                    else
                    {
                        ChannelName = _cmdParams[0];
                        ModeFlag = _cmdParams[1];
                        RequestType = ModeRequestType.SetChannelModes;
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        public string GenerateResponse(string modes)
        {
            return BuildMessageRPL("www.rspy.cc", $"MODE {ChannelName} {modes}", "");
        }
    }
}
