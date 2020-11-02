using Newtonsoft.Json;
using System.IO;

namespace UniSpyLib.RetroSpyConfig
{
    public class ConfigManager
    {

        public static UniSpyConfig Config { get; protected set; }

        static ConfigManager()
        {
            LoadConfigFile();
        }

        private static void LoadConfigFile()
        {
            using (StreamReader fstream = File.OpenText(@"UniSpyServer.cfg"))
            {
                Config = JsonConvert.DeserializeObject<UniSpyConfig>(fstream.ReadToEnd());
            }
        }
    }
}
