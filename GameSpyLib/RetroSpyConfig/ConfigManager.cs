using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace GameSpyLib.RetroSpyConfig
{
    public class ConfigManager
    {

        public static RetroSpyConfig Config { get; protected set; }

        static ConfigManager()
        {
            LoadConfigFile();
        }
        private static void LoadConfigFile()
        {
            FileStream fstream = File.OpenRead(@"RetroSpyServer.json");
            fstream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[(int)fstream.Length];
            fstream.Read(buffer, 0, (int)fstream.Length);
            string strBuffer = Encoding.UTF8.GetString(buffer);
            //stream.Position = 0;
            Config = JsonConvert.DeserializeObject<RetroSpyConfig>(strBuffer);
            fstream.Close();

        }
    }
}
