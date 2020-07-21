using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using System.Collections.Generic;
using System.Linq;

/////////////////////////Finished?/////////////////////////////////
namespace PresenceSearchPlayer.Handler.CommandHandler.Nick
{
    internal class NickHandlerDataModel
    {
        public string Nick;
        public string Uniquenick;
    }
    /// <summary>
    /// Uses a email and namespaceid to find all nick in this account
    /// </summary>
    public class NickHandler : PSPCommandHandlerBase
    {
        List<NickHandlerDataModel> nickDBResults;
        protected new NickRequestModel _request;
        public NickHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new NickRequestModel(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            try
            {
                using (var db = new retrospyContext())
                {
                    var result = from u in db.Users
                                 join p in db.Profiles on u.Userid equals p.Userid
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 where u.Email == _request.Email
                                 && u.Password ==_request.PassEnc
                                 && n.Namespaceid == _request.NamespaceID
                                 select new NickHandlerDataModel { Nick = p.Nick, Uniquenick = n.Uniquenick };

                    if (result.Count() == 0)
                    {
                        _errorCode = GPErrorCode.CheckBadPassword;
                    }

                    //we store data in strong type so we can use in next step
                    nickDBResults = result.ToList();
                }
            }
            catch
            {
                _errorCode = GPErrorCode.DatabaseError;
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = @"\nr\";

            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = ErrorMsg.BuildGPErrorMsg(_errorCode);
            }
            else
            {
                foreach (var info in nickDBResults)
                {
                    _sendingBuffer += @"\nick\";
                    _sendingBuffer += info.Nick;
                    _sendingBuffer += @"\uniquenick\";
                    _sendingBuffer += info.Uniquenick;
                }

                _sendingBuffer += @"\ndone";
            }
            base.ConstructResponse();
        }
    }
}
