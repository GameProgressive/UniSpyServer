using GameSpyLib.Logging;
using GameSpyLib.Encryption;
using GameSpyLib.MiscMethod;
using GameSpyLib.Network;
using StatsAndTracking.Entity.Structure;
using StatsAndTracking.Handler.CommandSwitcher;
using System.Collections.Generic;
using System;
using System.Text;

namespace StatsAndTracking
{
    public class GStatsSession : TemplateTcpSession
    {

        public PlayerData PlayerData { get; set; }

        public GStatsSession(TemplateTcpServer server) : base(server)
        {
            PlayerData = new PlayerData();
        }

        protected override void OnConnected()
        {
            SendAsync(GenerateServerChallenge());
            base.OnConnected();
        }
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (size < 2 && size > 2048)
            {
                return;
            }

            string plainText = Decrypt(buffer, offset, size);
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Debug, $"[Recv] {plainText}");

            OnReceived(plainText);
        }
        protected override void OnReceived(string message)
        {
           new StatsCommandSwitcher().Switch(this, message);
        }


        /// <summary>
        /// Encrypt and send async
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override bool SendAsync(string plainText)
        {
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Debug, $@"[Send] {plainText}\final\");
            
            return BaseSendAsync(Encrypt(plainText));
        }

        /// <summary>
        /// Generate server challenge for a new connected player
        /// </summary>
        /// <returns></returns>
        public string GenerateServerChallenge()
        {
            //response total length bigger than 38bytes
            // challenge length should be bigger than 20bytes
            PlayerData.Challenge = "00000000000000000000";
            return $@"\challenge\{PlayerData.Challenge}";
        }


        protected string Decrypt(byte[] buffer, long offset, long size)
        {
            byte[] temp = new byte[(int)size];
            Array.Copy(buffer, 0, temp, 0, (int)size);
            //remove final
            string cipherText = Encoding.ASCII.GetString(temp);
            cipherText = cipherText.Substring(0, cipherText.Length - 7);

            return XorEncoding.Encrypt(cipherText, XorEncoding.XorType.Type1) + @"\final\";
        }

        protected string Encrypt(string plainText)
        {
            return XorEncoding.Encrypt(plainText, XorEncoding.XorType.Type1) + @"\final\";
        }
    }
}
