using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSpyLib.Common
{
    public class gsAssert
    {
        /// <summary>
        /// This is the platform specific default assert condition handler
        /// </summary>
        /// <param name="DisplayMessage">Displays message and goes into an infinite loop</param>

        public void _gsDebugAssert(string DisplayMessage)
        {
            //To Do
            Console.WriteLine(DisplayMessage);
            while (true)
            {
            };
        }
        /// <summary>
        /// This is the platform specific default assert condition handler
        /// </summary>
        /// <param name="szErrot"></param>
        /// <param name="szText"></param>
        /// <param name="szFile"></param>
        /// <param name="line"></param>
        public void gsDebugAssert(string szErrot, string szText, string szFile, int line)
        {
            string buffer = szErrot+ szText+ szFile+line.ToString();

            _gsDebugAssert(buffer);
        }
    }
}
