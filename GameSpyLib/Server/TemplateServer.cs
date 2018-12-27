using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSpyLib.Server
{
    public abstract class TemplateServer
    {
        /// <summary>
        /// Indicates whether this object has been disposed yet
        /// </summary>
        public bool IsDisposed { get; protected set; } = false;

        public TemplateServer()
        {

        }

        ~TemplateServer()
        {
            if (!IsDisposed)
                Dispose();
        }

        /// <summary>
        /// Releases all Objects held by this server. Will also
        /// shutdown the server if its still running
        /// </summary>
        public abstract void Dispose();
    }
}
