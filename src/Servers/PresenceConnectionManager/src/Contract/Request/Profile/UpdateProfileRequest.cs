using System;
using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Enumerate;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Request
{

    public sealed class UpdateProfileRequest : RequestBase
    {
        public UpdateProfileRequest(string rawRequest) : base(rawRequest)
        {
        }

        public bool HasPublicMaskFlag { get; private set; }
        public PublicMasks PublicMask { get; private set; }

        public string SessionKey { get; private set; }
        public int PartnerID { get; private set; }
        public string Nick { get; private set; }
        public string Uniquenick { get; private set; }

        public bool HasFirstNameFlag { get; private set; }
        public string FirstName { get; private set; }
        public bool HasLastNameFlag { get; private set; }
        public string LastName { get; private set; }

        public bool HasICQFlag { get; private set; }
        public int ICQUIN { get; private set; }

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

            if (RequestKeyValues.ContainsKey("publicmask"))
            {
                PublicMasks mask;
                if (!Enum.TryParse(RequestKeyValues["publicmask"], out mask))
                {
                    throw new GPParseException("publicmask format is incorrect");
                }
                HasPublicMaskFlag = true;
                PublicMask = mask;
            }

            if (!RequestKeyValues.ContainsKey("sesskey"))
            {
                throw new GPParseException("sesskey is missing");
            }
            SessionKey = RequestKeyValues["sesskey"];

            if (RequestKeyValues.ContainsKey("firstname"))
            {
                FirstName = RequestKeyValues["firstname"];
                HasFirstNameFlag = true;
            }

            if (RequestKeyValues.ContainsKey("lastname"))
            {
                LastName = RequestKeyValues["lastname"];
                HasLastNameFlag = true;
            }

            if (RequestKeyValues.ContainsKey("icquin"))
            {
                int icq;
                if (!int.TryParse(RequestKeyValues["icquin"], out icq))
                {
                    throw new GPParseException("icquin format is incorrect");
                }
                HasICQFlag = true;
                ICQUIN = icq;
            }


            if (RequestKeyValues.ContainsKey("homepage"))
            {
                HasHomePageFlag = true;
                HomePage = RequestKeyValues["homepage"];
            }

            if (RequestKeyValues.ContainsKey("birthday"))
            {
                int date;

                if (int.TryParse(RequestKeyValues["birthday"], out date))
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
            if (RequestKeyValues.ContainsKey("sex"))
            {
                byte sex;

                if (!byte.TryParse(RequestKeyValues["sex"], out sex))
                {
                    throw new GPParseException("sex format is incorrect");
                }
                HasSexFlag = true;
                Sex = sex;
            }

            if (RequestKeyValues.ContainsKey("zipcode"))
            {
                HasZipCode = true;
                ZipCode = RequestKeyValues["zipcode"];
            }

            if (RequestKeyValues.ContainsKey("countrycode"))
            {
                HasCountryCode = true;
                CountryCode = RequestKeyValues["countrycode"];
            }

            if (RequestKeyValues.ContainsKey("partnerid"))
            {
                int partnerid;
                if (!int.TryParse(RequestKeyValues["partnerid"], out partnerid))
                {
                    throw new GPParseException("partnerid is incorrect");
                }
                PartnerID = partnerid;
            }

            if (RequestKeyValues.ContainsKey("nick"))
            {
                // throw new GPParseException("nickname is missing.");
                Nick = RequestKeyValues["nick"];
            }

            if (RequestKeyValues.ContainsKey("uniquenick"))
            {
                // throw new GPParseException("uniquenick is missing.");
                Uniquenick = RequestKeyValues["uniquenick"];
            }
        }
    }
}

