using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;
using System.Linq;
using System.Collections.Generic;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.Login;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;

namespace UniSpyServer.Servers.PresenceConnectionManager.Application
{
    public class StorageOperation : IStorageOperation
    {
        public static IStorageOperation Persistance = new StorageOperation();

        public void DeleteFriendByProfileId(int profileId, int targetId, int namespaceId)
        {
            using (var db = new UniSpyContext())
            {
                var result = db.Friends.Where(f => f.ProfileId == profileId
                                                   && f.Targetid == targetId
                                                   && f.Namespaceid == namespaceId).FirstOrDefault();
                if (result is not null)
                {
                    db.Friends.Remove(result);
                }
                else
                {
                    throw new GPDatabaseException("More than one buddy found in database, please check database.");
                }
            }
        }

        public List<int> GetBlockedProfileIds(int profileId, int namespaceId)
        {
            using (var db = new UniSpyContext())
            {
                return db.Blockeds.Where(f => f.ProfileId == profileId
                                  && f.Namespaceid == namespaceId)
                                  .Select(f => f.Targetid).ToList();
            }

        }
        public List<int> GetFriendProfileIds(int profileId, int namespaceId)
        {
            using (var db = new UniSpyContext())
            {
                return db.Friends.Where(f => f.ProfileId == profileId
                                          && f.Namespaceid == namespaceId).Select(f => f.Targetid).ToList();
            }
        }

        public GetProfileDataModel GetProfileInfos(int profileId, int namespaceId)
        {
            using (var db = new UniSpyContext())
            {
                //we have to make sure the search target has the same namespaceID
                var result = from p in db.Profiles
                             join s in db.Subprofiles on p.ProfileId equals s.ProfileId
                             join u in db.Users on p.Userid equals u.UserId
                             where p.ProfileId == profileId
                             && s.NamespaceId == namespaceId
                             select new GetProfileDataModel
                             {
                                 Nick = p.Nick,
                                 ProfileId = p.ProfileId,
                                 UniqueNick = s.Uniquenick,
                                 Email = u.Email,
                                 Firstname = p.Firstname,
                                 Lastname = p.Lastname,
                                 Icquin = p.Icquin,
                                 Homepage = p.Homepage,
                                 Zipcode = p.Zipcode,
                                 Countrycode = p.Countrycode,
                                 Longitude = p.Longitude,
                                 Latitude = p.Latitude,
                                 Location = p.Location,
                                 Birthday = p.Birthday,
                                 Birthmonth = p.Birthmonth,
                                 Birthyear = p.Birthyear,
                                 Sex = (byte)p.Sex,
                                 Publicmask = p.Publicmask,
                                 Aim = p.Aim,
                                 Picture = p.Picture,
                                 Occupationid = p.Occupationid,
                                 Industryid = p.Industryid,
                                 Incomeid = p.Incomeid,
                                 Marriedid = p.Marriedid,
                                 Childcount = p.Childcount,
                                 Interests1 = p.Interests1,
                                 Ownership1 = p.Ownership1,
                                 Connectiontype = p.Connectiontype,
                             };

                return result.FirstOrDefault();
            }
        }

        public (User, Profile, Subprofile) GetUsersInfos(string email, string nickName)
        {
            using (var db = new UniSpyContext())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.UserId equals p.Userid
                             join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                             where u.Email == email
                             && p.Nick == nickName
                             select new { u, p, n };

                var info = result.FirstOrDefault();
                if (info is null)
                {
                    throw new GPLoginBadProfileException("email and nick dose not exist");
                }
                return (info.u, info.p, info.n);
            }
        }

        public (User, Profile, Subprofile) GetUsersInfos(string uniqueNick, int namespaceId)
        {

            using (var db = new UniSpyContext())
            {
                var result = from n in db.Subprofiles
                             join p in db.Profiles on n.ProfileId equals p.ProfileId
                             join u in db.Users on p.Userid equals u.UserId
                             where n.Uniquenick == uniqueNick
                             && n.NamespaceId == namespaceId
                             select new { u, p, n };

                var info = result.FirstOrDefault();
                if (info is null)
                {
                    throw new GPLoginBadUniquenickException($"The uniquenick: {uniqueNick} is invalid.");
                }
                return (info.u, info.p, info.n);
            }
        }

        public (User, Profile, Subprofile) GetUsersInfos(string authToken, int partnerId, int namespaceId)
        {
            using (var db = new UniSpyContext())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.UserId equals p.Userid
                             join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                             where n.Authtoken == authToken
                             && n.PartnerId == partnerId
                             && n.NamespaceId == namespaceId
                             select new { u, p, n };

                var info = result.FirstOrDefault();
                if (info is null)
                {
                    throw new GPLoginBadPreAuthException("The pre-authentication token is invalid.");
                }
                return (info.u, info.p, info.n);
            }
        }

        public void UpdateBlockInfo(int targetId, int profileId, int namespaceId)
        {
            using (var db = new UniSpyContext())
            {
                if (db.Blockeds.Where(b => b.Targetid == targetId
                                           && b.Namespaceid == namespaceId
                                           && b.ProfileId == profileId).Count() == 0)
                {
                    Blocked blocked = new Blocked
                    {
                        ProfileId = profileId,
                        Targetid = targetId,
                        Namespaceid = namespaceId
                    };
                    db.Blockeds.Update(blocked);
                }
            }
        }

        public void UpdateFriendInfo(int targetId, int profileId, int namespaceId)
        {
            using (var db = new UniSpyContext())
            {
                if (db.Friends.Where(b => b.Targetid == targetId
                                           && b.Namespaceid == namespaceId
                                           && b.ProfileId == profileId).Count() == 0)
                {
                    Friend friend = new Friend
                    {
                        ProfileId = profileId,
                        Targetid = targetId,
                        Namespaceid = namespaceId
                    };
                    db.Friends.Update(friend);
                }
            }
        }
        public void AddNickName(int userId, int profileId, string newNick)
        {
            using (var db = new UniSpyContext())
            {

                var profiles = new UniSpyLib.Database.DatabaseModel.Profile
                {
                    ProfileId = profileId,
                    Nick = newNick,
                    Userid = userId
                };

                db.Add(profiles);
                db.SaveChanges();
            }
        }
        public void UpdateNickName(int profileId, string oldNick, string newNick)
        {
            using (var db = new UniSpyContext())
            {
                var result = from p in db.Profiles
                             where p.ProfileId == profileId
                             && p.Nick == oldNick
                             select p;

                if (result.Count() != 1)
                {
                    throw new GPDatabaseException("No user infomation found in database.");
                }
                else
                {
                    result.First().Nick = newNick;
                }

                var profile = db.Profiles.Where(p => p.ProfileId == profileId
                && p.Nick == oldNick).First();
                profile.Nick = newNick;
                db.Profiles.Add(profile);
                db.SaveChanges();
            }
        }

        public void UpdateProfileInfo(Profile profile)
        {
            using (var db = new UniSpyContext())
            {
                db.Update(profile);
                db.SaveChanges();
            }
        }

        public void UpdateUniqueNick(int subProfileId, string uniqueNick)
        {
            using (var db = new UniSpyContext())
            {
                var result = db.Subprofiles.FirstOrDefault(s => s.SubProfileId == subProfileId);
                result.Uniquenick = uniqueNick;
                db.Subprofiles.Update(result);
                db.SaveChanges();
            }
        }
        public void UpdateSubProfileInfo(Subprofile subprofile)
        {
            using (var db = new UniSpyContext())
            {
                db.Subprofiles.Update(subprofile);
                db.SaveChanges();
            }
        }
    }
}