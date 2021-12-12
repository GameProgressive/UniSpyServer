using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel
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
        GetChannelUserModes,
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
    [RequestContract("MODE")]
    public sealed class ModeRequest : ChannelRequestBase
    {
        public ModeRequestType RequestType { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public uint LimitNumber { get; set; }
        public string ModeFlag { get; set; }
        public string Password { get; set; }
        public ModeRequest(string rawRequest) : base(rawRequest)
        {
        }
        public ModeRequest() { }
        public override void Parse()
        {
            base.Parse();
            if (_cmdParams.Count == 1)
            {
                ChannelName = _cmdParams[0];
                RequestType = ModeRequestType.GetChannelModes;
            }
            else if (_cmdParams.Count == 2 || _cmdParams.Count == 3)
            {
                ModeFlag = _cmdParams[1];
                // TODO! We ignore multiple mode flags, maybe we need it in future for supporting games
                switch (ModeFlag)
                {
                    case "+q":
                        NickName = _cmdParams[0];
                        RequestType = ModeRequestType.EnableUserQuietFlag;
                        break;
                    case "-q":
                        NickName = _cmdParams[0];
                        RequestType = ModeRequestType.DisableUserQuietFlag;
                        break;
                    case "+k":
                        ChannelName = _cmdParams[0];
                        Password = _cmdParams[2];
                        RequestType = ModeRequestType.AddChannelPassword;
                        break;
                    case "-k":
                        ChannelName = _cmdParams[0];
                        Password = _cmdParams[2];
                        RequestType = ModeRequestType.RemoveChannelPassword;
                        break;
                    case "+l":
                        ChannelName = _cmdParams[0];
                        LimitNumber = uint.Parse(_cmdParams[2]);
                        RequestType = ModeRequestType.AddChannelUserLimits;
                        break;
                    case "-l":
                        ChannelName = _cmdParams[0];
                        RequestType = ModeRequestType.RemoveChannelUserLimits;
                        break;
                    case "+b":
                        ChannelName = _cmdParams[0];
                        NickName = _cmdParams[2];
                        RequestType = ModeRequestType.AddBanOnUser;
                        break;
                    case "-b":
                        ChannelName = _cmdParams[0];
                        RequestType = ModeRequestType.RemoveBanOnUser;
                        break;
                    case "+co":
                        ChannelName = _cmdParams[0];
                        UserName = _cmdParams[2];
                        RequestType = ModeRequestType.AddChannelOperator;
                        break;
                    case "-co":
                        ChannelName = _cmdParams[0];
                        UserName = _cmdParams[2];
                        RequestType = ModeRequestType.RemoveChannelOperator;
                        break;
                    case "+cv":
                        ChannelName = _cmdParams[0];
                        NickName = _cmdParams[2];
                        RequestType = ModeRequestType.EnableUserVoicePermission;
                        break;
                    case "-cv":
                        ChannelName = _cmdParams[0];
                        NickName = _cmdParams[2];
                        RequestType = ModeRequestType.DisableUserVoicePermission;
                        break;
                    default:
                        throw new Exception.Exception("Unknown mode request type.");
                }
            }
            else
            {
                throw new Exception.Exception("number of IRC parameters are incorrect.");
            }


            // if (_cmdParams.Count == 1)
            // {
            //     ChannelName = _cmdParams[0];
            //     RequestType = ModeRequestType.GetChannelModes;
            //     return;
            // }

            // if (_cmdParams.Count >= 2)
            // {
            //     if (_cmdParams[1].Length <= 3)
            //     {
            //         ModeFlag = _cmdParams[1];

            //         switch (ModeFlag)
            //         {
            //             case "+q":
            //                 NickName = _cmdParams[0];
            //                 RequestType = ModeRequestType.EnableUserQuietFlag;
            //                 break;
            //             case "-q":
            //                 NickName = _cmdParams[0];
            //                 RequestType = ModeRequestType.DisableUserQuietFlag;
            //                 break;
            //             case "+k":
            //                 ChannelName = _cmdParams[0];
            //                 Password = _cmdParams[2];
            //                 RequestType = ModeRequestType.AddChannelPassword;
            //                 break;
            //             case "-k":
            //                 ChannelName = _cmdParams[0];
            //                 Password = _cmdParams[2];
            //                 RequestType = ModeRequestType.RemoveChannelPassword;
            //                 break;
            //             case "+l":
            //                 ChannelName = _cmdParams[0];
            //                 LimitNumber = uint.Parse(_cmdParams[2]);
            //                 RequestType = ModeRequestType.AddChannelUserLimits;
            //                 break;
            //             case "-l":
            //                 ChannelName = _cmdParams[0];
            //                 RequestType = ModeRequestType.RemoveChannelUserLimits;
            //                 break;
            //             case "+b":
            //                 ChannelName = _cmdParams[0];
            //                 NickName = _cmdParams[2];
            //                 RequestType = ModeRequestType.AddBanOnUser;
            //                 break;
            //             case "-b":
            //                 ChannelName = _cmdParams[0];
            //                 RequestType = ModeRequestType.RemoveBanOnUser;
            //                 break;
            //             case "+co":
            //                 ChannelName = _cmdParams[0];
            //                 UserName = _cmdParams[2];
            //                 RequestType = ModeRequestType.AddChannelOperator;
            //                 break;
            //             case "-co":
            //                 ChannelName = _cmdParams[0];
            //                 UserName = _cmdParams[2];
            //                 RequestType = ModeRequestType.RemoveChannelOperator;
            //                 break;
            //             case "+cv":
            //                 ChannelName = _cmdParams[0];
            //                 NickName = _cmdParams[2];
            //                 RequestType = ModeRequestType.EnableUserVoicePermission;
            //                 break;
            //             case "-cv":
            //                 ChannelName = _cmdParams[0];
            //                 NickName = _cmdParams[2];
            //                 RequestType = ModeRequestType.DisableUserVoicePermission;
            //                 break;
            //         }
            //     }
            //     else
            //     {
            //         // "MODE <channel name> <mode flags> <limit number>"
            //         if (_cmdParams.Count == 3)
            //         {
            //             ChannelName = _cmdParams[0];
            //             ModeFlag = _cmdParams[1];
            //             LimitNumber = uint.Parse(_cmdParams[2]);
            //             RequestType = ModeRequestType.SetChannelModesWithUserLimit;
            //         }
            //         // "MODE <channel name> <mode flags>"
            //         else
            //         {
            //             ChannelName = _cmdParams[0];
            //             ModeFlag = _cmdParams[1];
            //             RequestType = ModeRequestType.SetChannelModes;
            //         }
            //     }
            //     throw new Exception.Exception("Unknown mode request type.");
            // }
            // throw new Exception.Exception("number of IRC parameters are incorrect.");
        }
    }
}
