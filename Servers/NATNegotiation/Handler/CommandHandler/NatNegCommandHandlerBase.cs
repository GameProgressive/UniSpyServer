using GameSpyLib.Logging;
using NatNegotiation;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using Serilog.Events;
using GameSpyLib.Common.Entity.Interface;
using NatNegotiation.Server;
using GameSpyLib.Common.BaseClass;

namespace NatNegotiation.Handler.CommandHandler
{
    public class NatNegCommandHandlerBase:CommandHandlerBase
    {
        protected NNErrorCode _errorCode = NNErrorCode.NoError;
        protected byte[] _sendingBuffer;
        protected InitPacket _initPacket;
        protected ConnectPacket _connPacket;
        protected ReportPacket _reportPacket;
        protected NatNegClientInfo _clientInfo;
        protected byte[] _recv;

        public NatNegCommandHandlerBase(IClient client, NatNegClientInfo clientInfo, byte[] recv):base(client)
        {
            _clientInfo = clientInfo;
            _recv = recv;
        }

        public override void Handle()
        {
            base.Handle();

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
            _client.Send(_sendingBuffer);
        }
    }
}
