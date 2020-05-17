using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Server;
using System;

namespace NatNegotiation.Handler.CommandHandler.CommandSwitcher
{
    public class NatNegCommandSwitcher
    {
        public void Switch(NatNegSession session, byte[] recv)
        {
            try
            {
                //BytesRecieved[7] is nnpacket.PacketType.
                switch ((NatPacketType)recv[7])
                {
                    case NatPacketType.Init:
                        new InitHandler(session, recv).Handle();
                        break;
                    case NatPacketType.AddressCheck:
                        new AddressCheckHandler(session, recv).Handle();
                        break;
                    case NatPacketType.NatifyRequest:
                        new NatifyHandler(session, recv).Handle();
                        break;
                    case NatPacketType.ConnectAck:
                        session.UserInfo.SetIsGotConnectAckPacketFlag();
                        break;
                    case NatPacketType.Report:
                        new ReportHandler(session, recv).Handle();
                        break;
                    case NatPacketType.ErtAck:
                        new ErtACKHandler(session, recv).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
            }
        }
    }
}
