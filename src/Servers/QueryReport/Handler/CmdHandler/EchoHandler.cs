using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using Serilog.Events;
using System;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace QueryReport.Handler.CmdHandler
{
    internal sealed class EchoHandler : QRCmdHandlerBase
    {
        private new EchoResult _result
        {
            get => (EchoResult)base._result;
            set => base._result = value;
        }
        public EchoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new EchoResult();
        }
        protected override void DataOperation()
        {
            //TODO prevent one pc create multiple game servers
            var searchKey = GameServerInfo.RedisOperator.BuildSearchKey(_session.RemoteIPEndPoint);
            var matchedKeys = GameServerInfo.RedisOperator.GetMatchedKeys(searchKey);
            if (matchedKeys.Count() != 1)
            {
                LogWriter.ToLog(LogEventLevel.Error, "Can not find game server");
                return;
            }
            //add recive echo packet on gameserverList

            //we get the first key in matchedKeys
            _result.GameServerInfo = GameServerInfo.RedisOperator.GetSpecificValue(matchedKeys[0]);

            _result.GameServerInfo.LastPacket = DateTime.Now;

            GameServerInfo.RedisOperator.SetKeyValue(matchedKeys[0], _result.GameServerInfo);
        }



        protected override void ResponseConstruct()
        {
            _response = new QRDefaultResponse(_request, _result);
        }
    }
}
