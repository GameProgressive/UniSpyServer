using System.Collections.Generic;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Contract.Request.Channel;

namespace UniSpy.Server.Chat.Aggregate
{
    public partial record Channel
    {
        [JsonProperty]
        public ChannelMode Mode { get; private set; } = new ChannelMode();

        /// <summary>
        /// We only care about how to set mode in this channel
        /// we do not need to care about if the user is legal
        /// because MODEHandler will check for us
        /// </summary>
        /// <param name="changer"></param>
        /// <param name="cmd"></param>
        public void SetProperties(ChannelUser changer, ModeRequest request)
        {
            // todo check permission of each operation
            foreach (var op in request.ModeOperations)
            {
                switch (op)
                {
                    case ModeOperationType.AddChannelUserLimits:
                        MaxNumberUser = request.LimitNumber;
                        break;
                    case ModeOperationType.RemoveChannelUserLimits:
                        MaxNumberUser = 200;
                        break;
                    case ModeOperationType.AddBanOnUser:
                        BanUser(request);
                        break;
                    case ModeOperationType.RemoveBanOnUser:
                        UnBanUser(request);
                        break;
                    case ModeOperationType.AddChannelPassword:
                        Password = request.Password;
                        break;
                    case ModeOperationType.RemoveChannelPassword:
                        Password = null;
                        break;
                    case ModeOperationType.AddChannelOperator:
                        AddChannelOperator(request);
                        break;
                    case ModeOperationType.RemoveChannelOperator:
                        RemoveChannelOperator(request);
                        break;
                    case ModeOperationType.EnableUserVoicePermission:
                        EnableUserVoicePermission(request);
                        break;
                    case ModeOperationType.DisableUserVoicePermission:
                        DisableUserVoicePermission(request);
                        break;
                    default:
                        Mode.SetChannelModes(op);
                        break;
                }
            }
        }
    }
}