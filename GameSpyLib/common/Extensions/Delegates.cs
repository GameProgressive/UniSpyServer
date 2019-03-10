using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSpyLib.Network;

namespace GameSpyLib
{
    public delegate void OnExceptionEvent(Exception exception);

    public delegate void DataRecivedEvent(TCPStream stream, string Message);

    public delegate void ConnectionClosed(TCPStream stream);
}
