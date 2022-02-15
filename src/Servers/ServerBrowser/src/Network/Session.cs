﻿using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.ServerBrowser.Abstraction;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Misc;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.Servers.ServerBrowser.Handler.CommandSwitcher;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace UniSpyServer.Servers.ServerBrowser.Network
{
    public sealed class Session : UniSpyTcpSession
    {
        public string GameSecretKey { get; set; }
        public string ClientChallenge { get; set; }
        public AdHocRequest AdHocMessage { get; set; }
        public EncryptionParameters EncParams { get; set; }
        public Session(UniSpyTcpServer server) : base(server)
        {
        }
        protected override void OnReceived(byte[] message) => new CmdSwitcher(this, message).Switch();
        protected override byte[] Encrypt(byte[] buffer)
        {
            SBEncryption enc;
            if (EncParams == null)
            {
                EncParams = new EncryptionParameters();
                enc = new SBEncryption(
                GameSecretKey,
                ClientChallenge,
                EncParams);
            }
            else
            {
                enc = new SBEncryption(EncParams);
            }
            // if the response is PushServerMessage, we encrypt hole message
            if (buffer[2] == 2)
            {
                return enc.Encrypt(buffer);
            }
            else
            {
                var cryptHeader = buffer.Take(14).ToArray();
                var cipherBody = enc.Encrypt(buffer.Skip(14).ToArray());
                return cryptHeader.Concat(cipherBody).ToArray();
            }
        }
    }
}
