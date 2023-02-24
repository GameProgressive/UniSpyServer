using Newtonsoft.Json;
using System.IO;
using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Core.Config
{
    public class ConfigManager
    {
        public static bool IsConfigFileExist => File.Exists(ConfigPath);
        public static UniSpyConfig Config = LoadConfigFile();
        public const string ConfigPath = @"UniSpyServerConfig.json";
        private static UniSpyConfig LoadConfigFile()
        {
            if (!IsConfigFileExist)
            {
                throw new UniSpyException("UniSpy server config file not found");
            }
            using (StreamReader fstream = File.OpenText(ConfigPath))
            {
                return JsonConvert.DeserializeObject<UniSpyConfig>(fstream.ReadToEnd());
            }
        }
    }
}
