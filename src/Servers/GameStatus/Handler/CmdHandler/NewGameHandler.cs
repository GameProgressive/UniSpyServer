using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Response;
using GameStatus.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CmdHandler
{
    internal sealed class NewGameHandler : GSCmdHandlerBase
    {
        // "\newgame\\sesskey\%d\challenge\%d";
        //"\newgame\\connid\%d\sesskey\%d"
        private new NewGameResult _result
        {
            get { return (NewGameResult)base._result; }
            set { base._result = value; }
        }
        public NewGameHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new NewGameResult();
        }
        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
        protected override void ResponseConstruct()
        {
            _response = new NewGameResponse(_request, _result);
        }
    }
}
