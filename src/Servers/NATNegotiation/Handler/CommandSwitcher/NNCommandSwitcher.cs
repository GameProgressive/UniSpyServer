using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Logging;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Handler.CommandHandler;
using NATNegotiation.Server;
using Serilog.Events;
using System;

namespace NATNegotiation.Handler.CommandSwitcher
{
    public class NNCommandSwitcher
    {
        public static void Switch(NatNegSession session, byte[] rawRequest)
        {

            IRequest request = NNRequestSerializer.Serialize(rawRequest);

            if (request == null)
            {
                return;
            }

            try
            {
                switch (((NNRequestBase)request).PacketType)
                {
                    case NatPacketType.Init:
                        new InitHandler(session, request).Handle();
                        break;
                    case NatPacketType.AddressCheck:
                        new AddressCheckHandler(session, request).Handle();
                        break;
                    case NatPacketType.NatifyRequest:
                        new NatifyHandler(session, request).Handle();
                        break;
                    case NatPacketType.ConnectAck:
                        new ConnectACKHandler(session, request).Handle();
                        break;
                    case NatPacketType.Report:
                        new ReportHandler(session, request).Handle();
                        break;
                    case NatPacketType.ErtAck:
                        new ErtAckHandler(session, request).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(rawRequest);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }
        }
    }
}
