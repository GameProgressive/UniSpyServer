using Newtonsoft.Json;
using System.IO;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.UniSpyLib.Config
{
    public class ConfigManager
    {

        public static UniSpyConfig Config => LoadConfigFile();
        public static readonly string ConfigPath = @"UniSpyServerConfig.json";
        private static UniSpyConfig LoadConfigFile()
        {
            if (!File.Exists(ConfigPath))
            {
                throw new UniSpyException("Config file not found");
            }
            using (StreamReader fstream = File.OpenText(ConfigPath))
            {
                return JsonConvert.DeserializeObject<UniSpyConfig>(fstream.ReadToEnd());
            }
        }
    }
}
