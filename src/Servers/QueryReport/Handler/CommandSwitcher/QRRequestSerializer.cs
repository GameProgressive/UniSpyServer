using System;
using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Logging;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Request;
using QueryReport.Handler.SystemHandler.ErrorMessage;
using Serilog.Events;

namespace QueryReport.Handler.CommandSwitcher
{
    public class QRRequestSerializer
    {
        public static IRequest Serilize(byte[] rawRequest)
        {
            IRequest request;

            switch ((QRPacketType)rawRequest[0])
            {
                case QRPacketType.AvaliableCheck:
                    request = new AvaliableRequest(rawRequest);
                    break;
                //verify challenge to check game server is real or fake;
                //after verify we can add game server to server list
                case QRPacketType.Challenge:
                    request = new ChallengeRequest(rawRequest);
                    break;
                case QRPacketType.HeartBeat:
                    request = new HeartBeatRequest(rawRequest);
                    break;
                case QRPacketType.KeepAlive:
                    request = new KeepAliveRequest(rawRequest);
                    break;
                case QRPacketType.EchoResponse:
                    request = new QRRequestBase(rawRequest);
                    break;
                case QRPacketType.ClientMessageACK:
                    request = new QRRequestBase(rawRequest);
                    break;
                default:
                    LogWriter.UnknownDataRecieved(rawRequest);
                    request = null;
                    break;
            }

            if (request == null)
            {
                return null;
            }

            if (!(bool)request.Parse())
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMessage(QRErrorCode.Parse));
                return null;
            }

            return request;
        }
    }
}
