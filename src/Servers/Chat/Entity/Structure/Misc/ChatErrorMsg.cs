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
                case ChatErrorCode.TooManyChannels:
                    break;
                
            }
        }
    }
}
