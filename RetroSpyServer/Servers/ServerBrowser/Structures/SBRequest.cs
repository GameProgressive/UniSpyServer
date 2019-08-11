using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.ServerBrowser.Structures
{
    public class SBRequest
    {
        public readonly byte ServerListRequest = 0;
        public readonly byte ServerInfoRequest = 1;
        public readonly byte SendMessageRequest = 2;
        public readonly byte KeepAlive = 3;

    }
}
