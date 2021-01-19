using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Entity.Structure.Misc
{
    internal sealed class ChatErrorMsg
    {
        public static void Build(ChatErrorCode errorCode, params string[] args )
        {
            switch (errorCode)
            {
                  case  ChatErrorCode.NoSuchNick:
                    break;
                case ChatErrorCode.NoSuchChannel:
                    break;
                case ChatErrorCode.TooManyChannels:
                    break;
                case ChatErrorCode.ErrOneUSNickName:
                    break;
                case ChatErrorCode.NickNameInUse:
                    break;
                case ChatErrorCode.MoreParameters:
                    break;
                case ChatErrorCode.ChannelIsFull:
                    break;
                case ChatErrorCode.InviteOnlyChannel:
                    break;
                case ChatErrorCode.BannedFromChannel:
                    break;
                case ChatErrorCode.BadChannelKey:
                    break;
                case ChatErrorCode.BadChanMask:
                    break;
                case ChatErrorCode.LoginFailed:
                    break;
                case ChatErrorCode.NoUniqueNick:
                    break;
                case ChatErrorCode.UniqueNIickExpired:
                    break;
                case ChatErrorCode.RegisterNickFailed:
                    break;

            }
        }
    }
}
