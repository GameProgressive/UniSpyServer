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
        public ChatServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
        }

        protected override TcpSession CreateSession() => new ChatSession(this);

    }
}
