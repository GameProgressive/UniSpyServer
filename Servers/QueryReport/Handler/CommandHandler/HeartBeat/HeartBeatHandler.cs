using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace QueryReport.Handler.CommandHandler.HeartBeat
{
    public class HeartBeatHandler : QRHandlerBase
    {

        public HeartBeatHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }


        protected override void CheckRequest(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO

            base.CheckRequest(server, endPoint, recv);
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO
            //Save server information.
            string dataPartition = Encoding.ASCII.GetString(recv.Skip(5).ToArray());
            string serverData, playerData, teamData;
            string[] dataFrag = dataPartition.Split(new string[] { "\x00\x00\x00", "\x00\x00\x02" }, StringSplitOptions.None);
            serverData = dataFrag[0];
            playerData = dataFrag[1];
            teamData = dataFrag[2];
            Console.WriteLine(serverData);
            Console.WriteLine(playerData);
            Console.WriteLine(teamData);

        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            ChallengePacket challenge = new ChallengePacket(endPoint, recv);
            _sendingBuffer = challenge.GenerateResponse();
        }
    }
}
