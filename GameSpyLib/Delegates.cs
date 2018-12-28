using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSpyLib.Gamespy.Net;

namespace GameSpyLib
{
    public delegate void OnExceptionEvent(Exception exception);

    public delegate void DataRecivedEvent(GamespyTcpStream stream, string Message);

    public delegate void ConnectionClosed(GamespyTcpStream stream);
}
