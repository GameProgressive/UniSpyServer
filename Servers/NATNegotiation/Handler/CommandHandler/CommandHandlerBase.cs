using NatNegotiation;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Structure;

namespace NATNegotiation.Handler.CommandHandler
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
            ConvertRequest(client, recv);
            if (_errorCode != NNErrorCode.NoError)
            {
                server.ToLog("Packet parse error!");
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

            server.Send(client.RemoteEndPoint, _sendingBuffer);
        }
    }
}
