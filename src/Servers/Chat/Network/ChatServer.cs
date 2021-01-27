using NetCoreServer;
using System;
using System.Collections.Concurrent;
using System.Net;
using UniSpyLib.Network;

namespace Chat.Network
{
    internal sealed class ChatServer : UniSpyTCPServerBase
    {
        public const string ServerDomain = "rspy.cc";
        //we hard coded random key here for simplisity
        public static readonly string ClientKey = "0000000000000000";
        public static readonly string ServerKey = "0000000000000000";
        public new ConcurrentDictionary<Guid, TcpSession> Sessions => base.Sessions;

        public ChatServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new ChatSession(this);
        }
    }
}
