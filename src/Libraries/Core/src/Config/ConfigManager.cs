using System;
using Newtonsoft.Json;
using System.IO;

namespace UniSpy.Server.Core.Config
{
    public class ConfigManager
    {
        public static bool IsConfigFileExist => File.Exists(ConfigPath);
        public static UniSpyConfig Config = LoadConfigFile();
        public const string ConfigPath = "UniSpyServerConfig.json";
        public const string EnvVarName = "UNISPYCONFIG";
        private static UniSpyConfig LoadConfigFile()
        {
            var config = Environment.GetEnvironmentVariable(EnvVarName);
            if (config is not null && config !="")
            {
                return JsonConvert.DeserializeObject<UniSpyConfig>(config);
            }
            
            if (!IsConfigFileExist)
            {
                throw new UniSpy.Exception("UniSpy server config file not found");
            }
            using (StreamReader fstream = File.OpenText(ConfigPath))
            {
                return JsonConvert.DeserializeObject<UniSpyConfig>(fstream.ReadToEnd());
            }
        }
    }
}
