using GameSpyLib.Logging;
using NatNegotiation;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using Serilog.Events;
using GameSpyLib.Common.Entity.Interface;
using NatNegotiation.Server;

namespace NatNegotiation.Handler.CommandHandler
{
    public class CommandHandlerBase
    {
        protected NNErrorCode _errorCode = NNErrorCode.NoError;
        protected byte[] _sendingBuffer;
        protected InitPacket _initPacket;
        protected ConnectPacket _connPacket;
        protected ReportPacket _reportPacket;
        protected NatNegClient _client;
        protected NatNegClientInfo _clientInfo;
        protected byte[] _recv;

        public CommandHandlerBase(IClient client, NatNegClientInfo clientInfo, byte[] recv)
        {
            _client = (NatNegClient)client.GetInstance();
            _clientInfo = clientInfo;
            _recv = recv;
        }

        public virtual void Handle()
        {
            LogWriter.LogCurrentClass(this);

            CheckRequest();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }

            DataOperation();

            ConstructResponse();

            Response();
        }

        protected virtual void CheckRequest()
        {
        }

        protected virtual void DataOperation()
        {
        }

        protected virtual void ConstructResponse()
        {
        }

        protected virtual void Response()
        {
            if (_sendingBuffer == null)
            {
                return;
            }
            _client.SendAsync(_sendingBuffer);
        }
    }
}
