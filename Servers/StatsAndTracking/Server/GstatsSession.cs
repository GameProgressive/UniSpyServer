using GameSpyLib.Common;
using GameSpyLib.Encryption;
using GameSpyLib.MiscMethod;
using GameSpyLib.Network;
using StatsAndTracking.Entity.Structure;
using StatsAndTracking.Handler.CommandSwitcher;
using System.Collections.Generic;

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

        protected override void OnReceived(string message)
        {
            message = Decrypt(message);

            if (message[0] != '\\')
            {
                return;
            }

            string[] recieved = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = GameSpyUtils.ConvertRequestToKeyValue(recieved);

            CommandSwitcher.Switch(this, dict);
        }

        public override long Send(byte[] buffer, long offset, long size)
        {
            return BaseSend(buffer, offset, size);
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            return BaseSendAsync(buffer, offset, size);
        }
        /// <summary>
        /// Encrypt and send
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override long Send(string text)
        {
            return base.Send(Encrypt(text));
        }

        /// <summary>
        /// Encrypt and send async
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override bool SendAsync(string text)
        {
            return base.SendAsync(Encrypt(text));
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

        /// <summary>
        /// Encrypt using XOR with type3 method
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {

            LogPlainText(plainText);
            string cipherText = XorEncoding.Encrypt(plainText, XorEncoding.XorType.Type1) + @"\final\";
            return cipherText;
        }

        /// <summary>
        /// Decrypt using XOR with type3 method
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Decrypt(string cipherText)
        {
            cipherText = cipherText.Substring(0, cipherText.Length - 7);
            string plainText = XorEncoding.Encrypt(cipherText, XorEncoding.XorType.Type1) + @"\final\";
            LogPlainText(plainText);
            return plainText;
        }
    }
}
