namespace UniSpyServer.Servers.Chat.Entity.Enumerate
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
