namespace QueryReport.Entity.Enumerate
{
    public enum RequestType : byte
    {
        //Client request
        Challenge = 0x01,
        HeartBeat = 0x03,
        ClientMessage = 0x06,
        AddError = 0x04,
        Echo = 0x02,
        KeepAlive = 0x08,
        AvaliableCheck = 0x09
    }
    public enum ResponseType : byte
    {
        //Server response
        Query = 0x00,
        Echo = 0x02,
        ADDError = 0x04,
        ClientMessage = 0x06,
        RequireIPVerify = 0x09,
        ClientRegistered = 0x0A,
    }
    public enum PacketType : byte
    {
        //Server response
        Query = 0x00,
        Challenge = 0x01,
        Echo = 0x02,
        ADDError = 0x04,
        ClientMessage = 0x06,
        RequireIPVerify = 0x09,
        ClientRegistered = 0x0A,

        //Client request
        HeartBeat = 0x03,
        EchoResponse = 0x05,
        ClientMessageACK = 0x07,
        KeepAlive = 0x08,
        AvaliableCheck = 0x09
    }

    public enum QRStateChange : byte
    {
        NormalHeartBeat = 0,
        GameModeChange = 1,
        ServerShutDown = 2,
        CanNotRecieveChallenge = 3
    }
}
