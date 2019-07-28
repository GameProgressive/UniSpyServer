using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RetroSpyServer.Servers.CDKEY
{
    /// <summary>
    /// This class is used for storing the cdkey and user which is using this cdkey
    /// </summary>
    public class ClientInfo
    {
        //Stores the cdkey which are using
        public string cdkey;
        //Stores the IP address which is using this cdkey
        public IPEndPoint remote;
    }
}
