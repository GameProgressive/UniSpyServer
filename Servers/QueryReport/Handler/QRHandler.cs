namespace QueryReport.Handler
{
    /// <summary>
    /// This class contians gamespy master udp server functions  which help cdkeyserver to finish the master tcp server functionality. 
    /// This class is used to simplify the functions in server class, separate the other utility function making  the main server logic clearer
    /// </summary>
    public class QRHandler
   {
        public static QRDBQuery DBQuery = null;
               

        public static string HeartBeatChallengeGen()
        {
            return "";
        }

    }
}
