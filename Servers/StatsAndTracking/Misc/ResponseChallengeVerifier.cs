using System;
using System.Collections.Generic;

namespace StatsAndTracking.Misc
{
    public class ResponseChallengeVerifier
    {
        public static bool VerifyResponse(Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
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
