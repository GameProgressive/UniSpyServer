using GameSpyLib.Logging;
using NatNegotiation;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using Serilog.Events;

namespace NatNegotiation.Handler.CommandHandler
{
    public class CommandHandlerBase
    {
        protected NNErrorCode _errorCode = NNErrorCode.NoError;
        protected byte[] _sendingBuffer;
        protected InitPacket _initPacket;
        protected ConnectPacket _connPacket;
        protected ReportPacket _reportPacket;

        public virtual void Handle(NatNegServer server, ClientInfo client, byte[] recv)
        {
            LogWriter.LogCurrentClass(this);

            ConvertRequest(client, recv);
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }

            ProcessInformation(client, recv);

            ConstructResponsePacket(client, recv);

            SendResponse(server, client);
        }

        protected virtual void ConvertRequest(ClientInfo client, byte[] recv)
        {
        }

        protected virtual void ProcessInformation(ClientInfo client, byte[] recv)
        {
        }

        protected virtual void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
        }

        protected virtual void SendResponse(NatNegServer server, ClientInfo client)
        {
            if (_sendingBuffer == null)
            {
                return;
            }
            server.SendAsync(client.RemoteEndPoint, _sendingBuffer);
        }
    }
}
