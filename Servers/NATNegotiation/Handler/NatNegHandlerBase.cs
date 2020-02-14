using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using GameSpyLib.Common;
using GameSpyLib.Logging;
using NATNegotiation.Entity.Structure;

namespace NatNegotiation.Handler
{
    public class NatNegHandlerBase
    {

        protected EndPoint _endPoint;
        protected byte[] _recv;
        protected byte[] _sendingBuffer;
        protected NNErrorCode _errorCode = NNErrorCode.NoError;

        public NatNegHandlerBase(EndPoint endPoint, byte[] recv)
        {
            _endPoint = endPoint;
            _recv = recv;
        }

        protected virtual void Handle(NatNegServer server, EndPoint endPoint, byte[] recv)
        {
            HandleBasicInfo();
            if (_errorCode != NNErrorCode.NoError)
            {
                server.ToLog(LogLevel.Error, _errorCode.ToString());
                return;
            }
            // only need to override one in each handler according to packet type
            HandleInitPacket();
            HandleConnectPacket();
            HandleReportPacket();
            if (_errorCode != NNErrorCode.NoError)
            {
                server.ToLog(LogLevel.Error, _errorCode.ToString());
                return;
            }
            SendResponse(server);
        }

        protected virtual void HandleBasicInfo() { }

        protected virtual void HandleInitPacket() { }
        protected virtual void HandleConnectPacket() { }
        protected virtual void HandleReportPacket() { }
        protected virtual void SendResponse(NatNegServer server)
        {
            if (_sendingBuffer.Length != 0&&_endPoint!=null)
                server.SendAsync(_endPoint, _sendingBuffer);
        }
    }
}
