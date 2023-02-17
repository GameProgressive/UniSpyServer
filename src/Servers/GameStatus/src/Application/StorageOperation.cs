using System.Collections.Generic;
using UniSpy.Server.GameStatus.Abstraction.Interface;
using UniSpy.Server.GameStatus.Entity.Exception;
using UniSpy.Server.Core.Database.DatabaseModel;
using System.Linq;
using UniSpy.Server.GameStatus.Entity.Enumerate;

namespace UniSpy.Server.GameStatus.Application
{
    internal sealed class StorageOperation : IStorageOperation
    {
        public static IStorageOperation Persistance = new StorageOperation();
        public void CreateNewGameData()
        {
            throw new System.NotImplementedException();
        }

        public void CreateNewPlayerData(Dictionary<string, string> playerData)
        {
            throw new System.NotImplementedException();
        }
        public void UpdatePlayerData(int profileId, PersistStorageType storageType, int dataIndex, Dictionary<string, string> data)
        {
            using (var db = new UniSpyContext())
            {

                var result = from p in db.Pstorages
                             where p.ProfileId == profileId
                             && p.Dindex == dataIndex
                             && p.Ptype == (int)storageType
                             select p;

                Pstorage ps;
                if (result.Count() == 0)
                {
                    //insert a new record in database
                    ps = new Pstorage();
                    ps.Dindex = dataIndex;
                    ps.ProfileId = profileId;
                    ps.Ptype = (int)storageType;
                    ps.Data = data;
                    db.Pstorages.Add(ps);
                }
                else if (result.Count() == 1)
                {
                    //update an existed record in database
                    ps = result.First();
                    ps.Data = data;
                }

                db.SaveChanges();
            }
        }

        public int GetProfileId(string token)
        {
            using (var db = new UniSpyContext())
            {
                var result = from s in db.Subprofiles
                             where s.Authtoken == token
                             select s.ProfileId;
                if (result.Count() != 1)
                {
                    throw new GSException("No records found in database by authtoken.");
                }
                return result.First();
            }
        }

        public int GetProfileId(string cdKey, string nickName)
        {
            using (var db = new UniSpyContext())
            {
                var result = from s in db.Subprofiles
                             join p in db.Profiles on s.ProfileId equals p.ProfileId
                             where s.Cdkeyenc == cdKey && p.Nick == nickName
                             select s.ProfileId;
                if (result.Count() != 1)
                {
                    throw new GSException("No records found in database by cdkey hash.");
                }
                return result.First();
            }
        }

        public int GetProfileId(int profileId)
        {
            using (var db = new UniSpyContext())
            {
                var result = from p in db.Profiles
                             where p.ProfileId == profileId
                             select p.ProfileId;
                if (result.Count() != 1)
                {
                    throw new GSException("No records found in database by profileid.");
                }
                return result.First();
            }
        }

        public Dictionary<string, string> GetPlayerData(int profileId, PersistStorageType storageType, int dataIndex)
        {
            using (var db = new UniSpyContext())
            {
                var result = from ps in db.Pstorages
                             where ps.Ptype == (int)storageType
                             && ps.Dindex == dataIndex
                             && ps.ProfileId == profileId
                             select ps.Data;

                if (result.Count() != 1)
                {
                    throw new GSException("No records found in database.");
                }
                return result.FirstOrDefault();
            }

        }
    }
}