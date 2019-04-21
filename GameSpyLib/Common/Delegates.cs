using System;
using GameSpyLib.Network;

namespace GameSpyLib.Common
{
    public delegate void OnExceptionEvent(Exception exception);

    public delegate void DataRecivedEvent(GamespyTcpStream stream, string Message);

    public delegate void ConnectionClosed(GamespyTcpStream stream);
}
