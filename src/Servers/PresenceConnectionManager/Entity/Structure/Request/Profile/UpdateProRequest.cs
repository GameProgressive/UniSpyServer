using UniSpyLib.MiscMethod;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class UpdateProRequest : PCMRequestBase
    {
        public UpdateProRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public bool HasPublicMaskFlag { get; private set; }
        public PublicMasks PublicMask { get; private set; }

        public bool HasFirstNameFlag { get; protected set; }
        public string FirstName { get; protected set; }
        public bool HasLastNameFlag { get; protected set; }
        public string LastName { get; protected set; }

        public bool HasICQFlag { get; protected set; }
        public uint ICQUIN { get; protected set; }

        public bool HasHomePageFlag { get; protected set; }
        public string HomePage { get; protected set; }


        public bool HasBirthdayFlag { get; protected set; }
        public int BirthDay { get; protected set; }
        public ushort BirthMonth { get; protected set; }
        public ushort BirthYear { get; protected set; }

        public bool HasSexFlag { get; protected set; }
        public byte Sex { get; protected set; }

        public bool HasZipCode { get; protected set; }
        public string ZipCode { get; protected set; }

        public bool HasCountryCode { get; protected set; }
        public string CountryCode { get; protected set; }

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }
            if (_recv.ContainsKey("publicmask"))
            {
                PublicMasks mask;
                if (!Enum.TryParse(_recv["publicmask"], out mask))
                {
                    return GPError.Parse;
                }
                HasPublicMaskFlag = true;
                PublicMask = mask;
            }

            if (_recv.ContainsKey("firstname"))
            {
                FirstName = _recv["firstname"];
                HasFirstNameFlag = true;
            }

            if (_recv.ContainsKey("lastname"))
            {
                LastName = _recv["lastname"];
                HasLastNameFlag = true;
            }

            if (_recv.ContainsKey("icquin"))
            {
                uint icq;
                if (!uint.TryParse(_recv["icquin"], out icq))
                {
                    return GPError.Parse;
                }
                HasICQFlag = true;
                ICQUIN = icq;
            }


            if (_recv.ContainsKey("homepage"))
            {
                HasHomePageFlag = true;
                HomePage = _recv["homepage"];
            }

            if (_recv.ContainsKey("birthday"))
            {
                int date;

                if (int.TryParse(_recv["birthday"], out date))
                {
                    int d = ((date >> 24) & 0xFF);
                    ushort m = (ushort)((date >> 16) & 0xFF);
                    ushort y = (ushort)(date & 0xFFFF);

                    if (GameSpyUtils.IsValidDate(d, m, y))
                    {
                        BirthDay = d;
                        BirthMonth = m;
                        BirthYear = y;
                    }
                }
            }
            if (_recv.ContainsKey("sex"))
            {
                byte sex;

                if (!byte.TryParse(_recv["sex"], out sex))
                {
                    return GPError.Parse;
                }
                HasSexFlag = true;
                Sex = sex;
            }

            if (_recv.ContainsKey("zipcode"))
            {
                HasZipCode = true;
                ZipCode = _recv["zipcode"];
            }

            if (_recv.ContainsKey("countrycode"))
            {
                HasCountryCode = true;
                CountryCode = _recv["countrycode"];
            }


            return GPError.NoError;
        }
    }
}

