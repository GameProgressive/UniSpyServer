using System.Collections.Generic;
using System.Text;
using GameSpyLib.Encryption;
using ServerBrowser.Entity.Enumerator;

namespace ServerBrowser.Handler.CommandHandler.TestHandler
{
    public class RuleTest
    {
        public RuleTest(SBSession session, byte[] recv)
        {
            List<byte> data = new List<byte>();
            byte[] ip = { 192, 168, 0, 100 };
            byte[] port = { 200, 0 };
            string[] keys = { "gravity", "rankingon"};
            string[] values = { "80", "1"};

            data.AddRange(ip);
            data.AddRange(port);

            data.Add(0);
            data.Add(0);

            //we do not know what data client needed so we just add this here
            data.Add((byte)(GameServerFlags.HasFullRulesFlag));

            //add public ip
            data.AddRange(new byte[] { 192, 168, 0, 100 });

            for (int i = 0; i < keys.Length; i++)
            {
                data.Add(0xff);
                data.AddRange(Encoding.ASCII.GetBytes(keys[i]));
                data.Add(0);
                data.AddRange(Encoding.ASCII.GetBytes(values[i]));
                data.Add(0);
            }

            //we do not know what data client needed so we just add this here
            data.Add((byte)(GameServerFlags.HasFullRulesFlag));
            //add public ip
            data.AddRange(new byte[] { 192, 168, 0, 200 });

            for (int i = 0; i < keys.Length; i++)
            {
                data.Add(0xff);
                data.AddRange(Encoding.ASCII.GetBytes(keys[i]));
                data.Add(0);
                data.AddRange(Encoding.ASCII.GetBytes(values[i]));
                data.Add(0);
            }

            //we do not know what data client needed so we just add this here
            data.AddRange(new byte[] { 0, 255, 255, 255, 255 });

            byte[] buffer = data.ToArray();
            string skey = "HA6zkS";

            EnctypeX enx = new EnctypeX();
           // byte[] sendingBuffer = enx.EncryptData(skey,session.Challenge,buffer,0);
            
           // session.SendAsync(sendingBuffer);
        }
    }
}
