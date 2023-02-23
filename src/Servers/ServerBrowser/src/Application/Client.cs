using System.Linq;
using UniSpy.Server.ServerBrowser.Enumerate;
using UniSpy.Server.ServerBrowser.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser.Application
{
    public sealed class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; set => base.Info = value; }
        private byte[] _incompleteBuffer;
        public Client(IConnection connection) : base(connection)
        {
            IsLogRaw = true;
            Info = new ClientInfo();
            // Crypto is init in ServerListHandler
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);

        protected override void OnReceived(object buffer)
        {
            var data = buffer as byte[];
            byte[] compeleteBuffer;
            if (((RequestType)data[2]) == RequestType.SendMessageRequest)
            {
                if (data.Length > 9)
                {
                    // complete sendmessage request received
                    compeleteBuffer = data;
                }
                else
                {
                    _incompleteBuffer = data;
                    return;
                }
            }
            else if (data.Take(6).SequenceEqual(NatNegotiation.Abstraction.BaseClass.RequestBase.MagicData))
            {
                if (_incompleteBuffer is not null)
                {
                    compeleteBuffer = _incompleteBuffer.Concat(data).ToArray();
                    _incompleteBuffer = null;
                }
                else
                {
                    // we ignore natneg message when _incompleteBuffer is null
                    return;
                }
            }
            else
            {
                compeleteBuffer = data;
            }

            base.OnReceived(compeleteBuffer);
        }
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