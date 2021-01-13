using UniSpyLib.MiscMethod;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    internal sealed class UpdateProRequest : PCMRequestBase
    {
        public UpdateProRequest(string rawRequest) : base(rawRequest)
        {
        }

        public bool HasPublicMaskFlag { get; private set; }
        public PublicMasks PublicMask { get; private set; }

        public bool HasFirstNameFlag { get; private set; }
        public string FirstName { get; private set; }
        public bool HasLastNameFlag { get; private set; }
        public string LastName { get; private set; }

        public bool HasICQFlag { get; private set; }
        public uint ICQUIN { get; private set; }

        public bool HasHomePageFlag { get; private set; }
        public string HomePage { get; private set; }


        public bool HasBirthdayFlag { get; private set; }
        public int BirthDay { get; private set; }
        public ushort BirthMonth { get; private set; }
        public ushort BirthYear { get; private set; }

        public bool HasSexFlag { get; private set; }
        public byte Sex { get; private set; }

        public bool HasZipCode { get; private set; }
        public string ZipCode { get; private set; }

        public bool HasCountryCode { get; private set; }
        public string CountryCode { get; private set; }

        public override void Parse()
        {
            base.Parse();
            if( ErrorCode != GPErrorCode.NoError)
            {
                return;
            }
            if (KeyValues.ContainsKey("publicmask"))
            {
                PublicMasks mask;
                if (!Enum.TryParse(KeyValues["publicmask"], out mask))
                {
                    ErrorCode = GPErrorCode.Parse; return;
                }
                HasPublicMaskFlag = true;
                PublicMask = mask;
            }

            if (KeyValues.ContainsKey("firstname"))
            {
                FirstName = KeyValues["firstname"];
                HasFirstNameFlag = true;
            }

            if (KeyValues.ContainsKey("lastname"))
            {
                LastName = KeyValues["lastname"];
                HasLastNameFlag = true;
            }

            if (KeyValues.ContainsKey("icquin"))
            {
                uint icq;
                if (!uint.TryParse(KeyValues["icquin"], out icq))
                {
                    ErrorCode = GPErrorCode.Parse; return;
                }
                HasICQFlag = true;
                ICQUIN = icq;
            }


            if (KeyValues.ContainsKey("homepage"))
            {
                HasHomePageFlag = true;
                HomePage = KeyValues["homepage"];
            }

            if (KeyValues.ContainsKey("birthday"))
            {
                int date;

                if (int.TryParse(KeyValues["birthday"], out date))
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
            if (KeyValues.ContainsKey("sex"))
            {
                byte sex;

                if (!byte.TryParse(KeyValues["sex"], out sex))
                {
                    ErrorCode = GPErrorCode.Parse; return;
                }
                HasSexFlag = true;
                Sex = sex;
            }

            if (KeyValues.ContainsKey("zipcode"))
            {
                HasZipCode = true;
                ZipCode = KeyValues["zipcode"];
            }

            if (KeyValues.ContainsKey("countrycode"))
            {
                HasCountryCode = true;
                CountryCode = KeyValues["countrycode"];
            }
        }
    }
}

