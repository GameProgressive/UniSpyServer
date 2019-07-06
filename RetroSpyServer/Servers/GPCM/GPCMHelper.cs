
using GameSpyLib.Common;
using GameSpyLib.Logging;
using RetroSpyServer.DBQueries;
using RetroSpyServer.Servers.GPCM.Enumerator;
using System;
using System.Collections.Generic;

namespace RetroSpyServer.Servers.GPCM
{
    /// <summary>
    /// This class contians gamespy GPCM functions  which help cdkeyserver to finish the GPCM functionality. 
    /// This class is used to simplify the functions in server class, separate the other utility function making  the main server logic clearer
    /// </summary>
    public class GPCMHelper
    {
        public static GPCMDBQuery DBQuery = null;

        public static void UpdateStatus(long timestamp, GPCMClient client)
        {
            DBQuery.UpdateStatus(timestamp, client.RemoteEndPoint.Address, client.PlayerInfo.PlayerId, (uint)client.PlayerInfo.PlayerStatus);
        }

        public static void ResetStatusAndSessionKey()
        {
            DBQuery.ResetStatusAndSessionKey();
        }

        /// <summary>
        /// Updates the Users Country code when sent by the client
        /// </summary>
        /// <param name="recv">Array of information sent by the server</param>
        public static void UpdateUser(GPCMClient client,Dictionary<string, string> Recv)
        {
            // Set clients country code
            if (!Recv.ContainsKey("sesskey"))
                return;

            ushort ssk;
            if (!ushort.TryParse(Recv["sesskey"], out ssk))
                return;

            if (ssk != client.SessionKey)
                return;

            string query = "UPDATE profiles SET";
            object[] passData = new object[22] {
                null, // publicmask : 0
                null, // firstname
                null, // lastname
                null, // icq
                null, // homepage
                null, // zipcode
                null, // countrycode
                null, // birthday
                null, // sex
                null, // aim
                null, // pic
                null, // occ
                null, // ind
                null, // inc
                null, // mar
                null, // chc
                null, // i1
                null, // nick
                null, // uniquenick
                null, // Bithmonth
                null, // Birthyear
                null  // ProfileID
            };

            if (Recv.ContainsKey("publicmask"))
            {
                PublicMasks mask;
                if (Enum.TryParse(Recv["publicmask"], out mask))
                {
                    if (client.PlayerInfo.PlayerPublicMask != mask)
                    {
                        query += ", publicmask=@P0";
                        client.PlayerInfo.PlayerPublicMask = mask;
                        passData[0] = mask;
                    }
                }
            }

            if (Recv.ContainsKey("firstname"))
            {
                if (Recv["firstname"] != client.PlayerInfo.PlayerFirstName)
                {
                    query += ", firstname=@P1";
                    client.PlayerInfo.PlayerFirstName = Recv["firstname"];
                    passData[1] = client.PlayerInfo.PlayerFirstName;
                }
            }

            if (Recv.ContainsKey("lastname"))
            {
                if (Recv["lastname"] != client.PlayerInfo.PlayerLastName)
                {
                    query += ", lastname=@P2";
                    client.PlayerInfo.PlayerFirstName = Recv["lastname"];
                    passData[2] = client.PlayerInfo.PlayerLastName;
                }
            }

            if (Recv.ContainsKey("icquin"))
            {
                int icq = 0;

                if (int.TryParse(Recv["icquin"], out icq))
                {
                    if (icq != client.PlayerInfo.PlayerICQ)
                    {
                        query += "icq=@P3 ";
                        client.PlayerInfo.PlayerICQ = icq;
                        passData[3] = icq;
                    }
                }
            }

            if (Recv.ContainsKey("homepage"))
            {
                if (Recv["homepage"] != client.PlayerInfo.PlayerHomepage)
                {
                    query += ", homepage=@P4";
                    client.PlayerInfo.PlayerHomepage = Recv["homepage"];
                    passData[4] = client.PlayerInfo.PlayerHomepage;
                }
            }

            if (Recv.ContainsKey("zipcode"))
            {
                if (Recv["zipcode"] != client.PlayerInfo.PlayerZIPCode)
                {
                    query += ", zipcode=@P5";
                    client.PlayerInfo.PlayerZIPCode = Recv["zipcode"];
                    passData[5] = client.PlayerInfo.PlayerZIPCode;
                }
            }

            if (Recv.ContainsKey("countrycode"))
            {
                if (Recv["countrycode"] != client.PlayerInfo.PlayerCountryCode)
                {
                    query += ", countrycode=@P6";
                    client.PlayerInfo.PlayerCountryCode = Recv["zipcode"];
                    passData[6] = client.PlayerInfo.PlayerCountryCode;
                }
            }

            if (Recv.ContainsKey("birthday"))
            {
                int date;
                if (int.TryParse(Recv["birthday"], out date))
                {
                    ushort d = (ushort)((date >> 24) & 0xFF);
                    ushort m = (ushort)((date >> 16) & 0xFF);
                    ushort y = (ushort)(date & 0xFFFF);

                    if (GamespyUtils.IsValidDate(d, m, y))
                    {
                        if (client.PlayerInfo.PlayerBirthday != d)
                        {
                            query += ", birthday=@P6";
                            passData[6] = d;
                            client.PlayerInfo.PlayerBirthday = d;
                        }

                        if (client.PlayerInfo.PlayerBirthmonth != m)
                        {
                            query += ", birthmonth=@P19";
                            passData[19] = m;
                            client.PlayerInfo.PlayerBirthmonth = m;
                        }

                        if (client.PlayerInfo.PlayerBirthyear != y)
                        {
                            query += ", birthyear=@P20";
                            passData[20] = y;
                            client.PlayerInfo.PlayerBirthyear = y;
                        }
                    }
                }


                if (Recv["countrycode"] != client.PlayerInfo.PlayerCountryCode)
                {
                    query += ", countrycode=@P7";
                    client.PlayerInfo.PlayerCountryCode = Recv["zipcode"];
                    passData[7] = client.PlayerInfo.PlayerCountryCode;
                }
            }


            if (Recv.ContainsKey("sex"))
            {
                PlayerSexType sex;
                if (Enum.TryParse(Recv["sex"], out sex))
                {
                    if (client.PlayerInfo.PlayerSex != sex)
                    {
                        query += "sex=@P8";
                        client.PlayerInfo.PlayerSex = sex;

                        if (client.PlayerInfo.PlayerSex == PlayerSexType.MALE)
                            passData[8] = "MALE";
                        else if (client.PlayerInfo.PlayerSex == PlayerSexType.FEMALE)
                            passData[8] = "FEMALE";
                        else
                            passData[8] = "PAT";
                    }
                }
            }

            if (Recv.ContainsKey("aim"))
            {
                if (Recv["aim"] != client.PlayerInfo.PlayerAim)
                {
                    query += ", aim=@P9";
                    client.PlayerInfo.PlayerAim = Recv["aim"];
                    passData[9] = client.PlayerInfo.PlayerAim;
                }
            }

            if (Recv.ContainsKey("pic"))
            {
                int pic = 0;

                if (int.TryParse(Recv["pic"], out pic))
                {
                    if (pic != client.PlayerInfo.PlayerPicture)
                    {
                        query += ", picture=@P10";
                        client.PlayerInfo.PlayerPicture = pic;
                        passData[10] = pic;
                    }
                }
            }

            if (Recv.ContainsKey("occ"))
            {
                int occ = 0;

                if (int.TryParse(Recv["occ"], out occ))
                {
                    if (occ != client.PlayerInfo.PlayerOccupation)
                    {
                        query += ", occupationid=@P11";
                        client.PlayerInfo.PlayerOccupation = occ;
                        passData[11] = occ;
                    }
                }
            }

            if (Recv.ContainsKey("ind"))
            {
                int ind = 0;

                if (int.TryParse(Recv["ind"], out ind))
                {
                    if (ind != client.PlayerInfo.PlayerIndustryID)
                    {
                        query += ", industryid=@P12";
                        client.PlayerInfo.PlayerIndustryID = ind;
                        passData[12] = ind;
                    }
                }
            }

            if (Recv.ContainsKey("inc"))
            {
                int inc = 0;

                if (int.TryParse(Recv["inc"], out inc))
                {
                    if (inc != client.PlayerInfo.PlayerIncomeID)
                    {
                        query += ", industryid=@P13";
                        client.PlayerInfo.PlayerIncomeID = inc;
                        passData[13] = inc;
                    }
                }
            }

            if (Recv.ContainsKey("mar"))
            {
                int mar = 0;

                if (int.TryParse(Recv["mar"], out mar))
                {
                    if (mar != client.PlayerInfo.PlayerMarried)
                    {
                        query += ", marriedid=@P14";
                        client.PlayerInfo.PlayerMarried = mar;
                        passData[14] = mar;
                    }
                }
            }

            if (Recv.ContainsKey("chc"))
            {
                int chc = 0;

                if (int.TryParse(Recv["chc"], out chc))
                {
                    if (chc != client.PlayerInfo.PlayerChildCount)
                    {
                        query += ", childcount=@P15";
                        client.PlayerInfo.PlayerChildCount = chc;
                        passData[15] = chc;
                    }
                }
            }

            if (Recv.ContainsKey("i1"))
            {
                int i1 = 0;

                if (int.TryParse(Recv["i1"], out i1))
                {
                    if (i1 != client.PlayerInfo.PlayerInterests)
                    {
                        query += ", interests1=@P16";
                        client.PlayerInfo.PlayerInterests = i1;
                        passData[16] = i1;
                    }
                }
            }

            if (Recv.ContainsKey("nick"))
            {
                if (Recv["nick"] != client.PlayerInfo.PlayerNick)
                {
                    query += ", nick=@P17";
                    client.PlayerInfo.PlayerNick = Recv["nick"];
                    passData[17] = client.PlayerInfo.PlayerNick;
                }
            }

            if (Recv.ContainsKey("uniquenick"))
            {
                if (Recv["uniquenick"] != client.PlayerInfo.PlayerUniqueNick)
                {
                    query += ", uniquenick=@P18";
                    client.PlayerInfo.PlayerHomepage = Recv["uniquenick"];
                    passData[18] = client.PlayerInfo.PlayerUniqueNick;
                }
            }

            if (query == "UPDATE profiles SET")
                return;

            query = query.Replace("SET,", "SET");

            passData[21] = client.PlayerInfo.PlayerId;
            query += " WHERE `profileid`=@P21";

            try
            {
                DBQuery.UpdateUserInfo(query,passData);
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
        }


        /// <summary>
        /// This method is called when the client requests for the Account profile
        /// </summary>
        public static void SendProfile(GPCMClient client,Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("profileid"))
            {
                GamespyUtils.SendGPError(client.Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            uint targetPID, messID;
            if (!uint.TryParse(dict["profileid"], out targetPID))
            {
                GamespyUtils.SendGPError(client.Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            if (!uint.TryParse(dict["id"], out messID))
            {
                GamespyUtils.SendGPError(client.Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            string datatoSend = @"\pi\\profileid\" + targetPID + @"\mp\4";

            // If the client want to access the public information
            // of another client
            if (targetPID != client.PlayerInfo.PlayerId)
            {
                uint publicMask;

                var Query = GPCMHelper.DBQuery.GetProfileInfo(targetPID);
                if (Query == null)
                {
                    GamespyUtils.SendGPError(client.Stream, 4, "Unable to get profile information.");
                    return;
                }

                if (!uint.TryParse(Query["publicmask"].ToString(), out publicMask))
                    publicMask = (uint)PublicMasks.MASK_NONE;

                datatoSend = string.Format(datatoSend + @"\nick\{0}\uniquenick\{1}\id\{2}", Query["nick"].ToString(), Query["uniquenick"].ToString(), messID);

                if (Query["email"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_EMAIL) > 0)
                        datatoSend += @"\email\" + Query["email"].ToString();
                }

                if (Query["lastname"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\lastname\" + Query["lastname"].ToString();

                if (Query["firstname"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\firstname\" + Query["firstname"].ToString();

                if (int.Parse(Query["icq"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\icquin\" + int.Parse(Query["icq"].ToString());

                if (client.PlayerInfo.PlayerHomepage.Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_HOMEPAGE) > 0)
                        datatoSend += @"\homepage\" + Query["homepage"].ToString();
                }

                if (uint.Parse(Query["picture"].ToString()) != 0)
                    datatoSend += @"\pic\" + uint.Parse(Query["Show"].ToString());

                if (Query["aim"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\aim\" + Query["aim"].ToString();

                if (int.Parse(Query["occupationid"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\occ\" + int.Parse(Query["occupationid"].ToString());

                if (Query["zipcode"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_ZIPCODE) > 0)
                        datatoSend += @"\zipcode\" + Query["zipcode"].ToString();
                }

                if (Query["countrycode"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_COUNTRYCODE) > 0)
                        datatoSend += @"\countrycode\" + Query["countrycode"].ToString();
                }

                if (ushort.Parse(Query["birthday"].ToString()) > 0 && ushort.Parse(Query["birthmonth"].ToString()) > 0 && ushort.Parse(Query["birthyear"].ToString()) > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_BIRTHDAY) > 0)
                        datatoSend += @"\birthday\" + (uint)((ushort.Parse(Query["birthday"].ToString()) << 24) | (ushort.Parse(Query["birthmonth"].ToString()) << 16) | ushort.Parse(Query["birthyear"].ToString()));
                }

                if (Query["location"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\loc\" + Query["location"].ToString();

                if (publicMask != (uint)PublicMasks.MASK_NONE && (publicMask & (uint)PublicMasks.MASK_SEX) > 0)
                {
                    PlayerSexType sexType;
                    if (Enum.TryParse(Query["sex"].ToString(), out sexType))
                    {
                        if (sexType == PlayerSexType.FEMALE)
                            datatoSend += @"\sex\1";
                        else if (sexType == PlayerSexType.MALE)
                            datatoSend += @"\sex\0";
                    }
                }

                if (float.Parse(Query["latitude"].ToString()) != 0.0f && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\lat\" + float.Parse(Query["latitude"].ToString());

                if (float.Parse(Query["longitude"].ToString()) != 0.0f && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\lon\" + float.Parse(Query["longitude"].ToString());

                if (int.Parse(Query["incomeid"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\inc\" + int.Parse(Query["incomeid"].ToString());

                if (int.Parse(Query["industryid"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\ind\" + int.Parse(Query["industryid"].ToString());

                if (int.Parse(Query["marriedid"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\mar\" + int.Parse(Query["marriedid"].ToString());

                if (int.Parse(Query["childcount"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\chc\" + int.Parse(Query["childcount"].ToString());

                if (int.Parse(Query["interests1"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\i1\" + int.Parse(Query["interests1"].ToString());

                if (int.Parse(Query["ownership1"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\o1\" + int.Parse(Query["ownership1"].ToString());

                if (int.Parse(Query["connectiontype"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\conn\" + int.Parse(Query["connectiontype"].ToString());

                // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
                datatoSend += @"\sig\" + GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex) + @"\final\";
            }
            else
            {
                // Since this is our profile, we have to see ALL informations that we can edit. This means that we don't need to check the public masks for sending
                // the data

                datatoSend = string.Format(datatoSend + @"\nick\{0}\uniquenick\{1}\email\{2}\id\{3}\pmask\{4}",
                                                                client.PlayerInfo.PlayerNick,
                                                                client.PlayerInfo.PlayerUniqueNick,
                                                                client.PlayerInfo.PlayerEmail,
                                                                /*(ProfileSent ? "5" : "2")*/ messID,
                                                                client.PlayerInfo.PlayerPublicMask
                                                                );

                if (client.PlayerInfo.PlayerLastName.Length > 0)
                    datatoSend += @"\lastname\" + client.PlayerInfo.PlayerLastName;

                if (client.PlayerInfo.PlayerFirstName.Length > 0)
                    datatoSend += @"\firstname\" + client.PlayerInfo.PlayerFirstName;

                if (client.PlayerInfo.PlayerICQ != 0)
                    datatoSend += @"\icquin\" + client.PlayerInfo.PlayerICQ;

                if (client.PlayerInfo.PlayerHomepage.Length > 0)
                    datatoSend += @"\homepage\" + client.PlayerInfo.PlayerHomepage;

                if (client.PlayerInfo.PlayerPicture != 0)
                    datatoSend += @"\pic\" + client.PlayerInfo.PlayerPicture;

                if (client.PlayerInfo.PlayerAim.Length > 0)
                    datatoSend += @"\aim\" + client.PlayerInfo.PlayerAim;

                if (client.PlayerInfo.PlayerOccupation != 0)
                    datatoSend += @"\occ\" + client.PlayerInfo.PlayerOccupation;

                if (client.PlayerInfo.PlayerZIPCode.Length > 0)
                    datatoSend += @"\zipcode\" + client.PlayerInfo.PlayerZIPCode;

                if (client.PlayerInfo.PlayerCountryCode.Length > 0)
                    datatoSend += @"\countrycode\" + client.PlayerInfo.PlayerCountryCode;

                if (client.PlayerInfo.PlayerBirthday > 0 && client.PlayerInfo.PlayerBirthmonth > 0 && client.PlayerInfo.PlayerBirthyear > 0)
                    datatoSend += @"\birthday\" + (uint)((client.PlayerInfo.PlayerBirthday << 24) | (client.PlayerInfo.PlayerBirthmonth << 16) | client.PlayerInfo.PlayerBirthyear);

                if (client.PlayerInfo.PlayerLocation.Length > 0)
                    datatoSend += @"\loc\" + client.PlayerInfo.PlayerLocation;

                if (client.PlayerInfo.PlayerSex == PlayerSexType.FEMALE)
                    datatoSend += @"\sex\1";
                else if (client.PlayerInfo.PlayerSex == PlayerSexType.MALE)
                    datatoSend += @"\sex\0";

                if (client.PlayerInfo.PlayerLatitude != 0.0f)
                    datatoSend += @"\lat\" + client.PlayerInfo.PlayerLatitude;

                if (client.PlayerInfo.PlayerLongitude != 0.0f)
                    datatoSend += @"\lon\" + client.PlayerInfo.PlayerLongitude;

                if (client.PlayerInfo.PlayerIncomeID != 0)
                    datatoSend += @"\inc\" + client.PlayerInfo.PlayerIncomeID;

                if (client.PlayerInfo.PlayerIndustryID != 0)
                    datatoSend += @"\ind\" + client.PlayerInfo.PlayerIndustryID;

                if (client.PlayerInfo.PlayerMarried != 0)
                    datatoSend += @"\mar\" + client.PlayerInfo.PlayerMarried;

                if (client.PlayerInfo.PlayerChildCount != 0)
                    datatoSend += @"\chc\" + client.PlayerInfo.PlayerChildCount;

                if (client.PlayerInfo.PlayerInterests != 0)
                    datatoSend += @"\i1\" + client.PlayerInfo.PlayerInterests;

                if (client.PlayerInfo.PlayerOwnership != 0)
                    datatoSend += @"\o1\" + client.PlayerInfo.PlayerOwnership;

                if (client.PlayerInfo.PlayerConnectionType != 0)
                    datatoSend += @"\conn\" + client.PlayerInfo.PlayerConnectionType;

                // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
                datatoSend += @"\sig\" + GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex) + @"\final\";

                // Set that we send the profile initially
                if (!client.ProfileSent) client.ProfileSent = true;
            }

            client.Stream.SendAsync(datatoSend);
        }
        /// <summary>
        /// Whenever the "newuser" command is recieved, this method is called to
        /// add the new users information into the database
        /// </summary>
        /// <param name="Recv">Array of parms sent by the server</param>
        public static void CreateNewUser(GPCMClient client,Dictionary<string, string> Recv)
        {
            // Make sure the user doesnt exist already
            try
            {

                // Check to see if user exists
                if (GPCMHelper.DBQuery.UserExists(Recv["nick"]))
                {
                    client.Stream.SendAsync(@"\error\\err\516\fatal\\errmsg\This account name is already in use!\id\1\final\");
                    client.Disconnect(DisconnectReason.CreateFailedUsernameExists);
                    return;
                }

                // We need to decode the Gamespy specific encoding for the password
                string Password = GamespyUtils.DecodePassword(Recv["passwordenc"]);
                string Cc = (client.RemoteEndPoint.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    //? GeoIP.GetCountryCode(RemoteEndPoint.Address)
                    //: "US";
                    ? "US" : "US";

                // Attempt to create account. If Pid is 0, then we couldnt create the account. TODO: Handle Unique Nickname
                if ((client.PlayerInfo.PlayerId = GPCMHelper.DBQuery.CreateUser(Recv["nick"], Password, Recv["email"], Cc, Recv["nick"])) == 0)
                {
                    GamespyUtils.SendGPError(client.Stream, 516, "An error oncurred while creating the account!");
                    client.Disconnect(DisconnectReason.CreateFailedDatabaseError);
                    return;
                }

                client.Stream.SendAsync(@"\nur\\userid\{0}\profileid\{0}\id\1\final\", client.PlayerInfo.PlayerId);
            }
            catch (Exception e)
            {
                // Check for invalid query params
                if (e is KeyNotFoundException)
                {
                    GamespyUtils.SendGPError(client.Stream, 516, "Invalid response received from the client!");
                }
                else
                {
                    GamespyUtils.SendGPError(client.Stream, 516, "An error oncurred while creating the account!");
                    LogWriter.Log.Write("An error occured while trying to create a new User account :: " + e.Message, LogLevel.Error);
                }

                client.Disconnect(DisconnectReason.GeneralError);
                return;
            }
        }

        public static void AddProducts(GPCMClient client,Dictionary<string, string> dictionary)
        {
            ushort readedSessionKey = 0;

            if (!dictionary.ContainsKey("products") || !dictionary.ContainsKey("sesskey"))
                return;

            if (!ushort.TryParse(dictionary["sesskey"], out readedSessionKey))
                return;

            if (readedSessionKey != client.SessionKey || readedSessionKey == 0)
                return;
        }


    }
}
