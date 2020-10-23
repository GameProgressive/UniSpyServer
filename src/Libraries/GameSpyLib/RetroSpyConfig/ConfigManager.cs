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
            using (StreamReader fstream = File.OpenText(@"RetroSpyServer.json"))
            {
                Config = JsonConvert.DeserializeObject<RetroSpyConfig>(fstream.ReadToEnd());
            }
        }
    }
}
