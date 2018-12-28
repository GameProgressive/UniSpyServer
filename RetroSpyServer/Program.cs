using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroSpyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServerFactory Emulator = new ServerFactory();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
