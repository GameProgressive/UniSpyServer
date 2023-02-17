using Newtonsoft.Json;
using System.IO;
using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Core.Config
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
