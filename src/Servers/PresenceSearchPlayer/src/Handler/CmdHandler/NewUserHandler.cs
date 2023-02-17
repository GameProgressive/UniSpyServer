using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Application;
using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerator;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.NewUser;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Database.DatabaseModel;

namespace UniSpy.Server.PresenceSearchPlayer.Handler.CmdHandler
{

    public class NewUserHandler : CmdHandlerBase
    {
        protected new NewUserRequest _request => (NewUserRequest)base._request;
        protected new NewUserResult _result { get => (NewUserResult)base._result; set => base._result = value; }

        public NewUserHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new NewUserResult();
        }

        protected override void DataOperation()
        {
            DatabaseOperationByType();
            UpdateOtherInfo();
        }

        private void UpdateOtherInfo()
        {
            if (_request.HasPartnerIDFlag)
            {
                _result.SubProfile.PartnerId = _request.PartnerID;
            }

            if (_request.HasProductIDFlag)
            {
                _result.SubProfile.ProductId = _request.ProductID;
            }

            if (_request.HasGameNameFlag)
            {
                _result.SubProfile.Gamename = _request.GameName;
            }

            if (_request.HasGamePortFlag)
            {
                _result.SubProfile.Port = _request.GamePort;
            }

            if (_request.HasCDKeyEncFlag)
            {
                _result.SubProfile.Cdkeyenc = _request.CDKey;
            }
            StorageOperation.Persistance.UpdateSubProfile(_result.SubProfile);
        }

        private void DatabaseOperationByType()
        {

            switch (NewUserStatus.CheckAccount)
            {
                case NewUserStatus.CheckAccount:
                    var user = StorageOperation.Persistance.GetUser(_request.Email);
                    if (user is null)
                    {
                        goto case NewUserStatus.AccountNotExist;
                    }
                    else
                    {
                        _result.User = user;
                        goto case NewUserStatus.AccountExist;
                    }

                case NewUserStatus.AccountNotExist:
                    _result.User = new User { Email = _request.Email, Password = _request.Password };
                    StorageOperation.Persistance.AddUser(_result.User);
                    goto case NewUserStatus.CheckProfile;

                case NewUserStatus.AccountExist:

                    if (_result.User.Password != _request.Password)
                    {
                        throw new GPNewUserBadPasswordException("password is incorrect when creating new user.");
                    }
                    else
                    {
                        goto case NewUserStatus.CheckProfile;
                    }

                case NewUserStatus.CheckProfile:
                    var profile = StorageOperation.Persistance.GetProfile(_result.User.UserId, _request.Nick);
                    if (profile is null)
                    {
                        goto case NewUserStatus.ProfileNotExist;
                    }
                    else
                    {
                        _result.Profile = profile;
                        goto case NewUserStatus.ProfileExist;
                    }

                case NewUserStatus.ProfileNotExist:
                    _result.Profile = new Profile { Userid = _result.User.UserId, Nick = _request.Nick };
                    StorageOperation.Persistance.AddProfile(_result.Profile);
                    goto case NewUserStatus.CheckSubProfile;

                case NewUserStatus.ProfileExist:
                //we do nothing here

                case NewUserStatus.CheckSubProfile:
                    var subProfile = StorageOperation.Persistance.GetSubProfile(_result.Profile.ProfileId, _request.NamespaceID, _request.ProductID);
                    if (subProfile is null)
                    {
                        goto case NewUserStatus.SubProfileNotExist;
                    }
                    else
                    {
                        _result.SubProfile = subProfile;
                        goto case NewUserStatus.SubProfileExist;
                    }

                case NewUserStatus.SubProfileNotExist:
                    //we create subprofile and return
                    _result.SubProfile = new Subprofile
                    {
                        ProfileId = _result.Profile.ProfileId,
                        Uniquenick = _request.Uniquenick,
                        NamespaceId = _request.NamespaceID
                    };
                    StorageOperation.Persistance.AddSubProfile(_result.SubProfile);
                    break;

                case NewUserStatus.SubProfileExist:
                    throw new GPNewUserUniquenickInUseException("unique nick is in use.");
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new NewUserResponse(_request, _result);
        }
    }
}
