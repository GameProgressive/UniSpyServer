﻿using GameSpyLib.Abstraction.Interface;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Abstraction;
using QueryReport.Entity.Abstraction.BaseClass;
using QueryReport.Server;
using System.Linq;
using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Handler.CommandHandler.Challenge
{
    public class ChallengeHandler : QRCommandHandlerBase
    {
        GameServer _gameServer;
        //we do not need to implement this to check the correctness of the challenge response
        public ChallengeHandler(ISession session, byte[] recv) : base(session, recv)
        {
        }

        protected override void DataOperation()
        {
            QRSession client = (QRSession)_session.GetInstance();
            var result =
                  GameServer.GetServers(client.RemoteEndPoint);

            if (result.Count() != 1)
            {
                _errorCode = QRErrorCode.Database;
                return;
            }
            _gameServer = result.First();
        }

        protected override void ConstructeResponse()
        {
            EchoPacket packet = new EchoPacket();
            packet.Parse(_recv);
            if (_session.InstantKey != packet.InstantKey)
            {
                _session.SetInstantKey(packet.InstantKey);
            }

            // We send the echo packet to check the ping
            _sendingBuffer = packet.BuildResponse();

            GameServer.UpdateServer(
                _session.RemoteEndPoint,
                _gameServer.ServerData.KeyValue["gamename"],
                _gameServer
                );
        }
    }
}
