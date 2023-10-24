using System.Text;
using System;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.QueryReport.Application
{
    public sealed class Client : ClientBase
    {
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            IsLogRaw = true;
            Info = new ClientInfo();
        }
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        protected override ISwitcher CreateSwitcher(object buffer)
        {
            var data = (byte[])buffer;
            if (data[0] == Convert.ToInt32('\\'))
            {
                return new V1.Handler.CmdSwitcher(this, UniSpyEncoding.GetString(data));
            }
            else
            {
                return new V2.Handler.CmdSwitcher(this, data);
            }
        }
    }
}