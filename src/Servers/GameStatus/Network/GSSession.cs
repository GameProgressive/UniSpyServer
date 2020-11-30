using UniSpyLib.Encryption;
using UniSpyLib.Logging;
using UniSpyLib.Network;
using Serilog.Events;
using GameStatus.Entity.Structure;
using GameStatus.Handler.CommandSwitcher;
using System;
using System.Text;

namespace GameStatus.Network
{
    public class GSSession : TCPSessionBase
    {

        public PlayerData PlayerData { get; set; }

        public GSSession(TCPServerBase server) : base(server)
        {
            PlayerData = new PlayerData();
        }

        /// <summary>
        /// When client connect, we send our challenge first
        /// </summary>
        protected override void OnConnected()
        {
            SendAsync($@"\challenge\{PlayerData.Challenge}");
            base.OnConnected();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (size < 2 && size > 2048)
            {
                LogWriter.ToLog(LogEventLevel.Error, "[Spam] client spam we ignored!");
                return;
            }

            string plainText = Decrypt(buffer, size);
            LogWriter.ToLog(LogEventLevel.Debug, $"[Recv] {plainText}");

            OnReceived(plainText);
        }

        protected override void OnReceived(string message)
        {
            new GSCommandSerializer(this, message).Serialize();
        }


        /// <summary>
        /// Encrypt and send async
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override bool SendAsync(string plainText)
        {
            LogWriter.ToLog(LogEventLevel.Debug, $@"[Send] {plainText}\final\");

            return BaseSendAsync(Encrypt(plainText));
        }



        private static string Decrypt(byte[] buffer, long size)
        {
            byte[] temp = new byte[(int)size];
            Array.Copy(buffer, 0, temp, 0, (int)size);
            //remove \final\, later we add final back
            string cipherText = Encoding.ASCII.GetString(temp);
            cipherText = cipherText.Substring(0, cipherText.Length - 7);

            return XorEncoding.Encrypt(cipherText, XorEncoding.XorType.Type1) + @"\final\";
        }

        private static string Encrypt(string plainText)
        {
            return XorEncoding.Encrypt(plainText, XorEncoding.XorType.Type1) + @"\final\";
        }
    }
}
