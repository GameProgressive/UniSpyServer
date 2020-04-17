using System;
namespace Chat.Entity.Enumerator
{
    public enum ChatChannelMode
    {
        InviteOnly,
        Private,
        Secret,
        Moderated,
        NoExternalMessages,
        OnlyOpsChangeTopic,
        OpsObeyChannelLimit,
    }
}
