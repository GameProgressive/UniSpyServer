using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(SBSession session, string recv)
        {
            string[] dataPartition = recv.Split(new string[] { "\x00\x00" }, StringSplitOptions.None);
            byte[] challenge= Encoding.ASCII.GetBytes(recv.Substring(0, 9));
            string[] gameInfo = recv.Substring(9).Split(new string[] { "\x00\\"}, StringSplitOptions.None);
            byte[] data = Encoding.ASCII.GetBytes(recv);
        }
    }
}
