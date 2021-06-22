using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;


namespace PresenceConnectionManager.Handler.CommandSwitcher
{
    internal class PCMRequestFactory : UniSpyRequestFactoryBase
    {
        private new string _rawRequest => (string)base._rawRequest;

        public PCMRequestFactory(object rawRequest) : base(rawRequest)
        {
        }

        public override IUniSpyRequest Deserialize()
        {
            // Read client message, and parse it into key value pairs
            var keyValues = GameSpyUtils.ConvertToKeyValue(_rawRequest);

            if (keyValues.Keys.Count < 1)
                return null; // malformed query

            var key = keyValues.Keys.First();

            if (!UniSpyServerFactoryBase.RequestMapping.ContainsKey(key))
            {
                LogWriter.LogUnkownRequest(_rawRequest);
                return null;
            }

            return (IUniSpyRequest)Activator.CreateInstance(UniSpyServerFactoryBase.RequestMapping[key], _rawRequest);
        }
    }
}
