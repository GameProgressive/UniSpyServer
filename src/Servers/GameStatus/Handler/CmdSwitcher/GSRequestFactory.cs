using GameStatus.Entity.Structure.Misc;
using GameStatus.Entity.Structure.Request;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;

namespace GameStatus.Handler.CmdSwitcher
{
    internal sealed class GSRequestFactory : UniSpyRequestFactoryBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        public GSRequestFactory(object rawRequest) : base(rawRequest)
        {
        }

        public override IUniSpyRequest Deserialize()
        {
            var keyValues = GameSpyUtils.ConvertToKeyValue(_rawRequest);
            switch (keyValues.Keys.First())
            {
                case GSRequestName.AuthenticateUser:
                    return new AuthRequest(_rawRequest);
                case GSRequestName.AuthenticatePlayer:
                    return new AuthPRequest(_rawRequest);
                case GSRequestName.GetProfileID:
                    return new GetPIDRequest(_rawRequest);
                case GSRequestName.GetPlayerData:
                    return new GetPDRequest(_rawRequest);
                case GSRequestName.SetPlayerData:
                    return new SetPDRequest(_rawRequest);
                case GSRequestName.UpdateGameData:
                    return new UpdGameRequest(_rawRequest);
                case GSRequestName.CreateNewGamePlayerData:
                    return new NewGameRequest(_rawRequest);
                default:
                    LogWriter.LogUnkownRequest(_rawRequest);
                    return null;
            }
        }
    }
}
