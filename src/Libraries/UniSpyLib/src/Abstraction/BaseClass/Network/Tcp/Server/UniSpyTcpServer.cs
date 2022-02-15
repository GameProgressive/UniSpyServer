using System;
using System.Net;
using System.Reflection;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server
{
    /// <summary>
    /// This is a template class that helps creating a TCP Server with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class UniSpyTcpServer : TcpServer, IServer
    {
        public Guid ServerID { get; private set; }
        /// <summary>
        /// Initialize TCP server with a given IP address and port number
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public UniSpyTcpServer(Guid serverID, IPEndPoint endpoint) : base(endpoint)
        {
            ServerID = serverID;

        }

        public override bool Start()
        {
            if (OptionSendBufferSize > int.MaxValue || OptionReceiveBufferSize > int.MaxValue)
            {
                throw new ArgumentException("Buffer size can not big than length of integer!");
            }
            return base.Start();
        }

        protected override TcpSession CreateSession()
        {
            var session = new UniSpyTcpSession(this);
            var n = Assembly.GetEntryAssembly().GetName().Name;
            var clientType = Assembly.GetEntryAssembly().GetType($"{n}.Entity.Structure.Client");
            var userInfoType = Assembly.GetEntryAssembly().GetType($"{n}.Entity.Structure.UserInfo");
            var userInfo = (UserInfoBase)Activator.CreateInstance(userInfoType, session.RemoteIPEndPoint);
            var client = (ClientBase)Activator.CreateInstance(clientType, new object[] { session, userInfo });
            return session;
        }
    }
}
