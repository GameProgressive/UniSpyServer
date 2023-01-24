using System.Linq;
using System.Net;
using UniSpyServer.Servers.ServerBrowser.V2.Handler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.ServerBrowser.Application
{
    public sealed class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; set => base.Info = value; }
        public Client(IConnection connection) : base(connection)
        {
            _isLogRawMessage = true;
            Info = new ClientInfo();
            // Crypto is init in ServerListHandler
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);

        // protected override void OnReceived(object buffer)
        // {
        //     // base.OnReceived(buffer);
        //     object rawRequest;
        //     if (((byte[])buffer).Take(2).SequenceEqual(UniSpyEncoding.GetBytes("\\")))
        //     {
        //         rawRequest = UniSpyEncoding.GetString((byte[])buffer);
        //         LogNetworkReceiving((string)rawRequest);
        //     }
        //     else
        //     {
        //         rawRequest = (byte[])buffer;
        //         LogNetworkReceiving((byte[])rawRequest);

        //     }
        //     var switcher = CreateSwitcher(rawRequest);
        //     switcher.Switch();
        // }
    }
}