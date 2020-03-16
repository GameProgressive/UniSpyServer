using System.Collections.Generic;
using GameSpyLib.Encryption;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.TestHandler
{
    public class ListTest
    {

        public ListTest(SBSession session, byte[] recv)
        {
            ServerListPacket sbRequest = new ServerListPacket(recv);
            //List<byte> data = new List<byte>();
            //byte[] ip = { 192, 168, 0, 100 };
            //byte[] port = { 200, 0 };

            //string[] keys = { "hostname", "numplayers", "maxplayers", "mapname", "gametype", "ping" };


            //string[] values1 = { "test server", "7", "16", "fuck map", "arena", "80" };
            //string[] values2 = { "test server2", "7", "16", "fuck map", "arena", "80" };

            //data.AddRange(ip);
            //data.AddRange(port);


            //data.Add((byte)keys.Length);
            //foreach (var key in keys)
            //{
            //    data.Add((byte)SBKeyType.String);
            //    data.AddRange(Encoding.ASCII.GetBytes(key));
            //    data.Add(0);
            //}

            ////this is unique keys number
            //data.Add(0);

            ////we do not know what data client needed so we just add this here
            //data.Add((byte)(GameServerFlags.HasKeysFlag));

            ////add public ip
            //data.AddRange(new byte[] { 192, 168, 0, 100 });

            //foreach (var value in values1)
            //{
            //    data.Add(0xff);
            //    data.AddRange(Encoding.ASCII.GetBytes(value));
            //    data.Add(0);
            //}

            //data.AddRange(new byte[] { 0,255, 255, 255, 255 });


            //byte[] buffer = data.ToArray();
            //string skey = "HA6zkS";

            //EnctypeX enx = new EnctypeX();
            //byte[] sendingBuffer = enx.EncryptData(skey, sbRequest.Challenge, buffer, 0);
            //session.Challenge = sbRequest.Challenge;
            List<byte> data = new List<byte>();
            string serverChallenge = "00000000";
            GOAEncryption enc = new GOAEncryption("HA6zkS", sbRequest.Challenge, serverChallenge);
            data.AddRange(enc.Encrypt("11111111"));
            session.SendAsync(data.ToArray());

        }
    }
}