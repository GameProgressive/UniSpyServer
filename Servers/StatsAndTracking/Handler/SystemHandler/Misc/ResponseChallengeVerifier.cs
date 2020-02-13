using System;
using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler.SystemHandler.Misc
{
    public class ResponseChallengeVerifier
    {
     
        public static bool VerifyResponse(string response, uint connid)
        {
            uint temp = connid & 0x38F371E6;
            string connstr = temp.ToString();
            string result = "";
           for(int i =0;i<connstr.Length;i++)
            {
                result += i + 17 + connstr[i];
            }
            return response == result ? true:false;
        }

        //        static char* value_for_key(const char* s, const char* key)
        //{

        //    static int valueindex;
        //        char* pos,*pos2;
        //	char keyspec[256] = "\\";
        //        static char value[2][256];

        //	valueindex ^= 1;
        //	strcat(keyspec, key);
        //        strcat(keyspec,"\\");
        //        pos = strstr(s, keyspec);
        //	if (!pos)
        //		return NULL;
        //	pos += strlen(keyspec);
        //        pos2 = value[valueindex];
        //	while (* pos && *pos != '\\')
        //		*pos2++ = *pos++;
        //	*pos2 = '\0';
        //	return value[valueindex];
        //}
    }
}
