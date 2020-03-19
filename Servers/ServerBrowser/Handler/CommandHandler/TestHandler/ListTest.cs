using System.Collections.Generic;
using System.Text;
using GameSpyLib.Encryption;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.TestHandler
{
    public class ListTest
    {
        public ListTest(SBSession session, byte[] recv)
        {
            ServerListPacket sbRequest = new ServerListPacket();
            sbRequest.Parse(recv);
            List<byte> data = new List<byte>();
            byte[] ip = { 192, 168, 0, 100 };
            byte[] port = { 200, 0 };

            string[] keys = { "hostname", "numplayers", "maxplayers", "mapname", "gametype", "ping", "localip0", "localip1", "localport" };

            string[] fullRuleKeys = { "gravity", "gamemode" };
            string[] fullRuleValues = { "80", "instant kill" };

            string[] values1 = { "test server", "7", "16", "fuck map", "arena", "80", "192.0.0.0", "192.168.0.1", "2000" };
            //string[] values2 = { "test server2", "7", "16", "fuck map", "arena", "80", "192.0.0.0", "192.168.0.1", "2000" };

            data.AddRange(ip);
            data.AddRange(port);

            data.Add((byte)keys.Length);

            foreach (var key in keys)
            {
                data.Add((byte)SBKeyType.String);
                data.AddRange(Encoding.ASCII.GetBytes(key));
                data.Add(0);
            }

            //this is unique keys number
            data.Add(0);

            //we do not know what data client needed so we just add this here
            data.Add((byte)GameServerFlags.HasKeysFlag);

            //add public ip
            data.AddRange(new byte[] { 192, 168, 0, 100 });

            foreach (var value in values1)
            {
                data.Add(0xff);
                data.AddRange(Encoding.ASCII.GetBytes(value));
                data.Add(0);
            }

            data.AddRange(new byte[] { 0, 255, 255, 255, 255 });

            List<byte> cryptHeader = new List<byte>();

            // we add the message length here
            cryptHeader.Add(2 ^ 0xEC);
            cryptHeader.AddRange(new byte[] { 0, 0 });
            string serverChallenge = "0000000000";
            cryptHeader.Add((byte)(serverChallenge.Length ^ 0xEA));
            cryptHeader.AddRange(Encoding.ASCII.GetBytes(serverChallenge));

            data.InsertRange(0, cryptHeader);

            GOAEncryption enc = new GOAEncryption("HA6zkS", sbRequest.Challenge, serverChallenge);
            data.AddRange(enc.Encrypt("11111111"));
            session.SendAsync(data.ToArray());
        }
    }
}
