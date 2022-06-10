using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Application.Network.Udp.Server
{
    /// <summary>
    /// This is a template class that helps creating a UDP Server with
    /// logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class UdpServer : NetCoreServer.UdpServer, IServer
    {
        public Guid ServerID { get; private set; }
        /// <summary>
        /// currently, we do not to care how to delete elements in dictionary
        /// </summary>
        public string ServerName { get; private set; }
        public static IDictionary<IPEndPoint, UdpSession> Sessions;
        IPEndPoint IServer.Endpoint => (IPEndPoint)Endpoint;
        static UdpServer()
        {
            Sessions = new ConcurrentDictionary<IPEndPoint, UdpSession>();
        }
        public UdpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(endpoint)
        {
            ServerID = serverID;
            ServerName = serverName;
        }
        public override bool Start()
        {
            if (OptionSendBufferSize > int.MaxValue || OptionReceiveBufferSize > int.MaxValue)
            {
                throw new ArgumentException("Buffer size can not big than length of integer!");
            }
            return base.Start();
        }

        protected override void OnStarted() => ReceiveAsync();

        /// <summary>
        /// Send unencrypted data
        /// </summary>
        /// <param name="buffer">plaintext</param>
        /// <returns>is sending succeed</returns>
        protected override void OnReceived(EndPoint endPoint, byte[] buffer, long offset, long size)
        {
            var session = CreateSession((IPEndPoint)endPoint);
            session.OnReceived(buffer.Skip((int)offset).Take((int)size).ToArray());
        }
        protected abstract IClient CreateClient(ISession session);
        protected virtual UdpSession CreateSession(IPEndPoint endPoint)
        {
            // we have to check if the endPoint is already in the dictionary,
            // which means the client is already in the dictionary, we do not need to create it
            // we just retrieve the session from the dictionary
            lock (Sessions)
            {
                if (Sessions.ContainsKey(endPoint))
                {
                    return Sessions[endPoint];
                }
                else
                {
                    var session = new UdpSession(this, endPoint);
                    CreateClient(session);
                    Sessions.Add(endPoint, session);
                    return session;
                }
            }
            // lock (ClientBase.ClientPool)
            // {
            //     if (ClientBase.ClientPool.ContainsKey(endPoint))
            //     {
            //         return (UdpSession)ClientBase.ClientPool[endPoint].Session;
            //     }
            //     else
            //     {
            //         // we create session and create client 
            //         // then we bind the event with client and session
            //         var session = new UdpSession(this, endPoint);
            //         var client = CreateClient(session);
            //         ClientBase.ClientPool.Add(endPoint, client);
            //         return session;
            //     }
        }
    }
}

